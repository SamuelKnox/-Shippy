using UnityEngine;

[RequireComponent(typeof(Follower))]
public class CameraController : MonoBehaviour
{
    [Tooltip("Background which scrolls with the camera")]
    [SerializeField]
    private Background background;

    private Follower follower;
    private Rigidbody2D targetBody2D;

    void Awake()
    {
        follower = GetComponent<Follower>();
    }

    void Update()
    {
        if (targetBody2D)
        {
            background.Scroll(targetBody2D.velocity * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        GameManager.OnPlayerJoined += OnPlayerJoined;
    }

    void OnDisable()
    {
        GameManager.OnPlayerJoined -= OnPlayerJoined;
    }

    /// <summary>
    /// Follows the player when joining the game
    /// </summary>
    /// <param name="player"></param>
    private void OnPlayerJoined(Player player)
    {
        follower.SetTarget(player.transform);
        targetBody2D = player.GetComponent<Rigidbody2D>();
        if (!targetBody2D)
        {
            Debug.LogError("Could not find Player Rigidbody2D!", player.gameObject);
            return;
        }
    }
}