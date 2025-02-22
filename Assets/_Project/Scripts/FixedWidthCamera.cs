using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedWidthCamera : MonoBehaviour
{
    // Задайте ширину уровня, на которой строится дизайн (например, 16)
    public float targetWidth = 16f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("Этот скрипт предназначен для ортографической камеры.");
        }
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        // Вычисляем orthographicSize так, чтобы ширина была фиксированной
        cam.orthographicSize = (targetWidth / cam.aspect) / 2f;
    }

    // Если экран может изменяться во время игры, можно обновлять размер камеры
    void Update()
    {
        UpdateCameraSize();
    }
}
