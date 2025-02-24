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

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.AddForce(forceDirection.normalized * forceMagnitude, forceMode);
        }
    }
}
