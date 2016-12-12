using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Background : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Scrolls the background's texture
    /// </summary>
    /// <param name="scroll">Amount to scroll background by</param>
    public void Scroll(Vector2 scroll)
    {
        float x = scroll.x / transform.localScale.x;
        float y = scroll.y / transform.localScale.y;
        meshRenderer.material.mainTextureOffset += new Vector2(x, y);
    }
}