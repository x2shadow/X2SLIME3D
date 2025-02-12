using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;

        public bool IsGrounded { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground")) // Убедись, что платформа имеет тег "Ground"
            {
                IsGrounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGrounded = false;
            }
        }

        public void Jump(float horizontalSpeed, float force)
        {
            Debug.Log("Jump!");
            rb.velocity = new Vector3(rb.velocity.x + horizontalSpeed, force, rb.velocity.z);
        }
    }
}
