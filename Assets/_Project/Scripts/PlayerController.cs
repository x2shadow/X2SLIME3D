using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] InputReader input;

        void Start()
        {
            input.EnablePlayerActions();
        }

        void OnEnable()
        {
            input.Jump += OnJump;
        }

        void OnDisable()
        {
            input.Jump -= OnJump;
        }

        void OnJump()
        {
            Debug.Log("Jump");
        }
    }
}
