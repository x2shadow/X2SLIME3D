using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;

        public void Jump(float force)
        {
            Debug.Log("Jump!");
            rb.velocity = new Vector3(rb.velocity.x + 1f, force, rb.velocity.z);
        }
    }
}
