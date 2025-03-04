using UnityEngine;

public class TextureTilingController : MonoBehaviour
{
    public float textureUnitSize = 2f;

    void Start()
    {
        UpdateTiling();
    }

    void UpdateTiling()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 platformSize = renderer.bounds.size;
            float tilingY = platformSize.x / textureUnitSize;
            renderer.materials[1].mainTextureScale = new Vector2(1f, tilingY);
        }
    }
}
