using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Tooltip("Start Button")]
    [SerializeField]
    private Button startButton;

    private string userName;

    public void Begin()
    {
        PhotonNetwork.player.name = userName;
        PhotonNetwork.player.SetScore(1);
        SceneManager.LoadScene(SceneNames.GamePlay);
    }

    public void OnNameChanged(string name)
    {
        userName = name;
        startButton.interactable = name != string.Empty;
    }
}