using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Delegate for player dying
    /// </summary>
    public delegate void PlayerDeath(Player player);

    /// <summary>
    /// Event called when player dies
    /// </summary>
    public static event PlayerDeath OnPlayerDeath;

    [Tooltip("How fast the player moves")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float moveSpeed = 5.0f;

    [Tooltip("Speed at which the player rotates when turning")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float turnSpeed = 5.0f;

    [Tooltip("Maximum speed at which the player can move")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float maxMoveSpeed = 25.0f;

    [Tooltip("Scoreboard for player")]
    [SerializeField]
    private TextMeshPro scoreboard;

    [Tooltip("Name for player")]
    [SerializeField]
    private TextMeshPro playerName;

    [Tooltip("Body which is rotated")]
    [SerializeField]
    private Transform body;

    //private string userName;
    private Rigidbody2D body2D;
    private PhotonView photonView;
    private Weapon weapon;
    //private int score = 1;

    private bool wasHit;

    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        weapon = GetComponentInChildren<Weapon>();
        if (!weapon)
        {
            Debug.LogError("Player does not have a weapon!", gameObject);
        }
        if (photonView.isMine)
        {
            playerName.text = PhotonNetwork.player.name;
        }
    }

    void Start()
    {
        StartCoroutine(UpdateGUI());
    }

    private IEnumerator UpdateGUI()
    {
        if (photonView.isMine)
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                photonView.RPC("SetGUI", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    private void SetGUI()
    {
        if (photonView.isMine)
        {
            scoreboard.text = PhotonNetwork.player.GetScore().ToString();
            playerName.text = PhotonNetwork.player.name;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasHit)
        {
            var projectile = collision.GetComponent<Projectile>();
            if (projectile)
            {
                Debug.Log(projectile.GetOwnerID());

                wasHit = true;

                var player = PhotonNetwork.playerList.Where(p => p.ID == projectile.GetOwnerID()).First();
                projectile.AwardOwnerPoints(player.GetScore());
                projectile.Die();
                Die();
            }
        }
    }

    /// <summary>
    /// Gets the player's Photon View
    /// </summary>
    /// <returns>Player's Photon View</returns>
    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    /// <summary>
    /// Gets the name of the user
    /// </summary>
    /// <returns>User's name</returns>
    public string GetUserName()
    {
        return PhotonNetwork.player.name;
    }

    /// <summary>
    /// Sets the user's name
    /// </summary>
    /// <param name="userName">Human name of user</param>
    public void SetUserName(string userName)
    {
        //this.userName = userName;
        if (photonView.isMine)
        {
            PhotonNetwork.player.name = userName;
        }
        playerName.text = userName;
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    /// <param name="thrust">Power with which to move the player</param>
    public void Move(float thrust)
    {
        var force = thrust * body.up * moveSpeed;
        body2D.AddForce(force);
        if (body2D.velocity.magnitude > maxMoveSpeed)
        {
            body2D.velocity = body2D.velocity.normalized * maxMoveSpeed;
        }
    }

    /// <summary>
    /// Gets the player's score
    /// </summary>
    /// <returns>Current player score</returns>
    public int GetScore()
    {
        return PhotonNetwork.player.GetScore();
    }

    /// <summary>
    /// Sets the player's score
    /// </summary>
    /// <param name="score">Current score</param>
    public void SetScore(int score)
    {
        //this.score = score;
        if (photonView.isMine)
        {
            PhotonNetwork.player.SetScore(score);
        }
        scoreboard.text = score.ToString();
    }

    /// <summary>
    /// Turns the player
    /// </summary>
    /// <param name="turn">Degrees to rotate</param>
    public void Rotate(float turn)
    {
        float rotation = -turn * turnSpeed;
        body.Rotate(0.0f, 0.0f, rotation);
    }

    /// <summary>
    /// Gets the player's unique identifier
    /// </summary>
    /// <returns>Player ID</returns>
    public int GetID()
    {
        return photonView.viewID;
    }

    /// <summary>
    /// Player dies
    /// </summary>
    private void Die()
    {
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath(this);
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}