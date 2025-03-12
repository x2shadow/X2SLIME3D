using UnityEngine;

public class AirTextureTilingController : MonoBehaviour
{
    public float textureUnitSizeX = 0.5f;
    public float textureUnitSizeY = 2f;

    float tilingX;
    float tilingY;

    void Update()
    {
        UpdateTiling();
    }

    void UpdateTiling()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 platformSize = renderer.bounds.size;
            if (platformSize.y >= platformSize.x)
            {
                tilingY = platformSize.x / textureUnitSizeX;
                tilingX = platformSize.y / textureUnitSizeY;
            }
            if (platformSize.y < platformSize.x)
            {
                tilingY = platformSize.x / textureUnitSizeY;
                tilingX = platformSize.y / textureUnitSizeX;
            }
            renderer.materials[0].mainTextureScale = new Vector2(tilingX, tilingY);
        }
    }
}
