using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private const string ProjectileContainerName = "Projectiles";

    [Tooltip("Project fired from weapon")]
    [SerializeField]
    private Projectile projectile;

    [Tooltip("Force at which the projectile fires")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float force = 5.0f;

    private static Transform projectileContainer;

    void Awake()
    {
        var player = GetComponentInParent<Player>();
        if (!player)
        {
            Debug.LogError("Weapon could not find Player in parent!", gameObject);
            return;
        }
    }

    void Start()
    {

        if (!projectileContainer)
        {
            projectileContainer = new GameObject(ProjectileContainerName).transform;
        }
    }

    /// <summary>
    /// Fires the Projectile
    /// </summary>
    public void Fire()
    {
        var projectileGameObject = PhotonNetwork.Instantiate(ResourceNames.Projectile, transform.position, transform.rotation, 0, null);
        var projectileInstance = projectileGameObject.GetComponent<Projectile>();
        if (!projectileInstance)
        {
            Debug.LogError("Could not find Projectile!", projectileGameObject);
            return;
        }
        projectileInstance.transform.SetParent(projectileContainer);
        projectileInstance.SetOwnerID(PhotonNetwork.player.ID);
        var projectileBody = projectileInstance.GetComponent<Rigidbody2D>();
        if (!projectileBody)
        {
            Debug.LogError("Projectile is missing a Rigidbody2D!", projectileInstance.gameObject);
            return;
        }
        projectileBody.velocity = transform.up * force;
    }


}