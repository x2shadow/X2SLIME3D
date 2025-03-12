using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AirStream : MonoBehaviour
{
    [Tooltip("Направление силы воздуха")]
    public Vector3 forceDirection = Vector3.forward;

    [Tooltip("Сила, с которой действует поток воздуха")]
    public float forceMagnitude = 10f;

    [Tooltip("Тип применения силы (Force, Acceleration, Impulse и т.д.)")]
    public ForceMode forceMode = ForceMode.Force;

    [Tooltip("Скорость прокрутки текстуры")]
    public float textureScrollSpeedMultiplier = 0.1f;

    private Renderer rend;
    
    void Start()
    {
        // Получаем компонент Renderer для работы с материалом
        rend = GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("Renderer не найден на объекте!");
        }
    }

    void Update()
    {
        // Если Renderer найден, смещаем текстуру для эффекта движения
        if (rend != null)
        {
            Vector2 offset = rend.material.mainTextureOffset;
            if(forceDirection.x > 0) offset.x -= textureScrollSpeedMultiplier * forceMagnitude * Time.deltaTime;
            if(forceDirection.x < 0) offset.x += textureScrollSpeedMultiplier * forceMagnitude * Time.deltaTime;
            if(forceDirection.y > 0) offset.y -= textureScrollSpeedMultiplier * forceMagnitude * Time.deltaTime;
            if(forceDirection.y < 0) offset.y += textureScrollSpeedMultiplier * forceMagnitude * Time.deltaTime;
            rend.material.mainTextureOffset = offset;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.AddForce(forceDirection.normalized * forceMagnitude, forceMode);
        }
    }
}
