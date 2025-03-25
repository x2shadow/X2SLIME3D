using System;
using R3;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerView : MonoBehaviour
    {
        ColorPalette palette;

        [SerializeField] private Rigidbody rb;
        public Renderer playerRenderer; 

        [Header("Ground Check (SphereCast)")]
        [SerializeField] private float groundCheckDistance = 1f;
        [SerializeField] private LayerMask groundLayer;

        private readonly ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<bool> isChargingJump = new ReactiveProperty<bool>(); 

        public ReadOnlyReactiveProperty<bool> IsGroundedObservable => isGrounded;
        public ReadOnlyReactiveProperty<bool> IsChargingJumpObservable => isChargingJump;
        public bool IsGrounded => isGrounded.Value;

        public Observable<Unit> OnCollisionImpact => onCollisionImpact.AsObservable();
        private readonly Subject<Unit> onCollisionImpact = new Subject<Unit>();

        private void Awake()
        {
            palette = Resources.Load<ColorPalette>("ColorPalette");
        }

        private void Start()
        {
            isChargingJump.Subscribe(charging =>
            {
                playerRenderer.material.color = charging ? palette.characterJumpColor : palette.characterColor;
            });
        }

        private void FixedUpdate()
        {
            bool currentlyGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);

            // Обновляем `isGrounded`, но только если значение изменилось (чтобы не триггерить подписчиков лишний раз)
            if (isGrounded.Value != currentlyGrounded)
            {
                isGrounded.Value = currentlyGrounded;
            }
        }

        public void SetChargingJump(bool isCharging) => isChargingJump.Value = isCharging;

        public void Jump(float forwardForce, float jumpForce)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(new Vector3(forwardForce, jumpForce, 0f), ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 8f) // Проверяем силу удара
            {
                onCollisionImpact.OnNext(Unit.Default);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
        }
    }
}
