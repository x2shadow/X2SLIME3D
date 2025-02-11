using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;
using R3;

namespace X2SLIME3D
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "X2SLIME3D/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions, IUIActions
    {        
        PlayerInputActions inputActions;

        public readonly Subject<Unit> JumpStart = new Subject<Unit>();
        public readonly Subject<Unit> JumpEnd   = new Subject<Unit>();
        public readonly Subject<Unit> UIButtonClicked = new Subject<Unit>();

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
                inputActions.UI.SetCallbacks(this);
            }
            EnablePlayerActions();
        }

        void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.Player.SetCallbacks(null); 
                inputActions.UI.SetCallbacks(null); 
            }
        }

        public void EnableUIActions()
        {
            inputActions.UI.Enable();
            inputActions.Player.Disable();
        }

        public void EnablePlayerActions()
        {
            inputActions.Player.Enable();
            inputActions.UI.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpStart.OnNext(Unit.Default);
                //Debug.Log("PlayerInput: JumpStart");
            }

            if (context.canceled) JumpEnd.OnNext(Unit.Default);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            { 
                UIButtonClicked.OnNext(Unit.Default); 
                //Debug.Log("UIInput: UIButtonClicked"); 
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }
    }
}
