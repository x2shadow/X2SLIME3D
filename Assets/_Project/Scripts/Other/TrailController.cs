using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField] private ParticleSystem trailParticleSystem;
    [SerializeField] private float speedThreshold = 8f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Включаем эмиссию, если скорость больше порога
        if (rb.velocity.magnitude > speedThreshold)
        {
            if (!trailParticleSystem.isEmitting)
                trailParticleSystem.Play();
        }
        else
        {
            if (trailParticleSystem.isEmitting)
                trailParticleSystem.Stop();
        }
    }
}
