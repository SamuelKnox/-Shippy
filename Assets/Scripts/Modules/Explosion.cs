using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    void Start()
    {
        StartCoroutine(FinishExploding());
    }

    private IEnumerator FinishExploding()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
