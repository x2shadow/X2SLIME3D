using UnityEngine;

public class CameraZAdjuster : MonoBehaviour
{
    // Значение Z для ландшафтного режима (16:9)
    public float zPositionLandscape = -20f;
    // Значение Z для портретного режима (9:16)
    public float zPositionPortrait = -55f;

    void Start()
    {
        AdjustCameraPosition();
    }

    //void Update()
    //{
    //    AdjustCameraPosition();
    //}

    void AdjustCameraPosition()
    {
        // Вычисляем текущее соотношение сторон экрана
        float aspect = (float)Screen.width / Screen.height;
        Vector3 pos = transform.position;

        // Если соотношение сторон >= 1 (ширина больше высоты) — считаем, что это 16:9
        if (aspect >= 1f)
        {
            pos.z = zPositionLandscape;
        }
        else
        {
            pos.z = zPositionPortrait;
        }
        
        transform.position = pos;
    }
}
