using UnityEngine;

public static class PhotonViewUtility
{
    /// <summary>
    /// Gets a player from its Photon View ID
    /// </summary>
    /// <param name="id">ID used to get player</param>
    /// <returns>Player</returns>
    public static Player GetPlayerFromViewID(int id)
    {
        if (id == 0)
        {
            return null;
        }
        var photonView = PhotonView.Find(id);
        if (!photonView)
        {
            Debug.LogError("Could not find Player Photon View!");
            return null;
        }
        var playerGameObject = photonView.gameObject;
        if (!playerGameObject)
        {
            Debug.LogError("Could not find Player GameObject!", photonView.gameObject);
            return null;
        }
        var player = playerGameObject.GetComponent<Player>();
        if (!player)
        {
            Debug.LogError("Could not find Player!", playerGameObject);
            return null;
        }
        return player;
    }
}