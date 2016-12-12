using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    private const string ScoreboardHeader = "Leaders:";

    private static GameManager instance;

    /// <summary>
    /// Gets the singleton instance of the Manager
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    /// <summary>
    /// Delegate for player joining game
    /// </summary>
    public delegate void PlayerJoined(Player player);

    /// <summary>
    /// Event called when player joins game
    /// </summary>
    public static event PlayerJoined OnPlayerJoined;

    [Tooltip("Scoreboard to display player scores")]
    [SerializeField]
    private TextMeshProUGUI scoreboard;

    [Tooltip("How often to update the scoreboard in seconds")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float scoreboardUpdateFrequency = 1.0f;

    [Tooltip("How long to wait before respawning a player after death")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float respawnDelay = 3.0f;

    [Tooltip("Circle boundaries of the level")]
    [SerializeField]
    private CircleCollider2D boundaries;

    //private List<Player> players = new List<Player>();
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        StartCoroutine(UpdateScoreboard());
    }

    void OnEnable()
    {
        Player.OnPlayerDeath += OnPlayerDeath;
    }

    void OnDisable()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
    }

    /// <summary>
    /// Spawns a new player ship
    /// </summary>
    public void SpawnPlayer()
    {
        var spawnPosition = Random.insideUnitCircle * boundaries.radius * boundaries.transform.localScale.magnitude / 2.0f;
        var playerGameObject = PhotonNetwork.Instantiate(ResourceNames.Player, spawnPosition, Quaternion.identity, 0);
        var player = playerGameObject.GetComponent<Player>();
        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(player);
        }
    }

    /// <summary>
    /// Reacts to the player's death
    /// </summary>
    /// <param name="player">Player who died</param>
    private void OnPlayerDeath(Player player)
    {
        var playerPhotonView = player.GetPhotonView();
        if (playerPhotonView.isMine)
        {
            player.SetScore(1);
            StartCoroutine(RespawnPlayer());
        }
    }

    /// <summary>
    /// Respawns the player after a delay
    /// </summary>
    /// <returns>Unity required IEnumerator</returns>
    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnPlayer();
    }

    /// <summary>
    /// Updates the scoreboard to display the current scores
    /// </summary>
    private IEnumerator UpdateScoreboard()
    {
        while (true)
        {
            yield return new WaitForSeconds(scoreboardUpdateFrequency);
            scoreboard.text = ScoreboardHeader;
            var players = PhotonNetwork.playerList.OrderByDescending(p => p.GetScore());
            foreach (var player in players)
            {
                scoreboard.text += "\n" + player.name + " - " + player.GetScore();
            }
        }
    }
}