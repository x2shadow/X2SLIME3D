using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace X2SLIME3D
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "X2SLIME3D/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {        
        PlayerInputActions inputActions;

        public event UnityAction Jump  = delegate { };

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
            }
        }
    
        void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.Player.SetCallbacks(null); 
            }
        }


        public void EnablePlayerActions()
        {
            inputActions.Enable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed) Jump.Invoke();
        }
    }
}
