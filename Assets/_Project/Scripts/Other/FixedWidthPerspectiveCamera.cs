using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedWidthPerspectiveCamera : MonoBehaviour
{
    // Задайте желаемую ширину уровня (например, 16 единиц)
    public float targetWidth = 16f;
    // Расстояние от камеры до плоскости уровня (например, 10 единиц)
    public float targetDistance = 10f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam.orthographic)
        {
            Debug.LogWarning("Скрипт FixedWidthPerspectiveCamera предназначен для перспективной камеры.");
        }
        UpdateCameraFOV();
    }

    void UpdateCameraFOV()
    {
        // Вычисляем горизонтальный угол обзора
        // horizontalAngle = 2 * arctan((targetWidth/2) / targetDistance)
        float horizontalAngleRad = 2f * Mathf.Atan((targetWidth / 2f) / targetDistance);
        // Вычисляем вертикальный угол обзора с учётом текущего соотношения сторон
        // verticalFOV = 2 * arctan(tan(horizontalAngle/2) / Camera.aspect)
        float verticalFOVRad = 2f * Mathf.Atan(Mathf.Tan(horizontalAngleRad / 2f) / cam.aspect);
        // Преобразуем угол в градусы и устанавливаем fieldOfView
        cam.fieldOfView = verticalFOVRad * Mathf.Rad2Deg;
    }

    // Если соотношение сторон может меняться во время игры (например, при повороте устройства), обновляем поле зрения каждый кадр
    void Update()
    {
        UpdateCameraFOV();
        //
    }
}
