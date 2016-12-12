using UnityEngine;

public class JoinGameResponse : MonoBehaviour
{
    public void OnJoinedRoom()
    {
        GameManager.Instance.SpawnPlayer();
    }
}