using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private PhotonView photonView;
    private Weapon weapon;

    void Awake()
    {
        player = GetComponent<Player>();
        photonView = GetComponent<PhotonView>();
        weapon = GetComponentInChildren<Weapon>();
        if (!weapon)
        {
            Debug.LogError("Could not find Player's Weapon in child!", gameObject);
            return;
        }
    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetButtonDown(InputNames.Fire))
            {
                weapon.Fire();
            }
        }
    }

    void FixedUpdate()
    {
        if (photonView.isMine)
        {
            Move();
            Rotate();
        }
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    private void Move()
    {
        float thrust = Input.GetAxis(InputNames.Vertical);
        player.Move(thrust);
    }

    /// <summary>
    /// Rotates the ship
    /// </summary>
    private void Rotate()
    {
        float turn = Input.GetAxis(InputNames.Horizontal);
        player.Rotate(turn);
    }
}