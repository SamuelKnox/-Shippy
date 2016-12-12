using UnityEngine;

/// <summary>
/// Plays the provided Particle System when the GameObject is destroyed
/// </summary>

public class EffectOnDestroy : MonoBehaviour
{
    [Tooltip("The GameObject which will be spawned when this game object is destroyed")]
    [SerializeField]
    private GameObject effectPrefab;
    
    private bool applicationClosing;


    void OnDestroy()
    {
        if (applicationClosing)
        {
            return;
        }

        Instantiate(effectPrefab, transform.position, Quaternion.identity);
    }

    void OnApplicationQuit()
    {
        applicationClosing = true;
    }

}