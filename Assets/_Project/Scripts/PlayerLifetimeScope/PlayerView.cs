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
            
            // Сбрасываем вертикальную скорость, чтобы прыжки были предсказуемыми
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Применяем один импульс сразу для горизонтального и вертикального движения
            Vector3 jumpForce = new Vector3(horizontalSpeed, force, 0f);
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }
    }
}
