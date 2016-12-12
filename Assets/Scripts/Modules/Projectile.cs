using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Projectile : MonoBehaviour
{
    private int ownerID = 0;
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Gets the owner's ID of this Projectile
    /// </summary>
    /// <returns>Projectile owner's ID</returns>
    public int GetOwnerID()
    {
        return ownerID;
    }

    /// <summary>
    /// Sets the owner ID of the Projectile
    /// </summary>
    /// <param name="owner">Projectile owner ID</param>
    public void SetOwnerID(int ownerID)
    {

        photonView.RPC("OnNewOwnerID", PhotonTargets.All, PhotonNetwork.player.ID);
    }

    /// <summary>
    /// Sets the owner ID on the Projectile for all clients
    /// </summary>
    /// <param name="ownerGuid">Owner ID to set</param>
    [PunRPC]
    public void OnNewOwnerID(int id)
    {
        ownerID = id;
    }

    /// <summary>
    /// Adds points to the Projectile owner's score
    /// </summary>
    /// <param name="points">Points to award</param>
    public void AwardOwnerPoints(int points)
    {
        if (photonView.isMine)
        {
            var player = PhotonNetwork.playerList.Where(p => p.ID == ownerID).First();
            player.SetScore(player.GetScore() + 1);
        }
    }

    /// <summary>
    /// Destroys the projectile
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }
}