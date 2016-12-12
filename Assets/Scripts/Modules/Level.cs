using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Level : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<Player>();
        if (player)
        {
            var playerBody = player.GetComponent<Rigidbody2D>();
            if (!playerBody)
            {
                Debug.LogError("Player does not have a Rigidbody2D!", gameObject);
                return;
            }
            playerBody.velocity *= -1.0f;
        }
        var projectile = collision.GetComponent<Projectile>();
        if (projectile)
        {
            projectile.Die();
        }
    }
}