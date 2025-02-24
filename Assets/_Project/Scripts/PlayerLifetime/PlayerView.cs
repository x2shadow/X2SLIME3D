using System;
using R3;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Renderer playerRenderer; 

        [Header("Ground Check (SphereCast)")]
        [SerializeField] private float groundCheckDistance = 1f;
        [SerializeField] private LayerMask groundLayer;

        private readonly ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<bool> isChargingJump = new ReactiveProperty<bool>(); 

        public ReadOnlyReactiveProperty<bool> IsGroundedObservable => isGrounded;
        public ReadOnlyReactiveProperty<bool> IsChargingJumpObservable => isChargingJump;
        public bool IsGrounded => isGrounded.Value;

        private void Start()
        {
            // Подписка на изменение `isChargingJump`
            isChargingJump.Subscribe(charging =>
            {
                playerRenderer.material.color = charging ? Color.yellow : Color.green;
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

        void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
        }
    }
}
