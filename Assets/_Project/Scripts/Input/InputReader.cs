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

        // Флаг для отслеживания, что прыжок уже начат
        private bool jumpStarted = false;

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
                inputActions.UI.SetCallbacks(this);
            }
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
                // При нажатии проверяем, что указатель не над UI.
                if (!UIChecker.IsPointerOverUI)
                {
                    jumpStarted = true;
                    JumpStart.OnNext(Unit.Default);
                    //Debug.Log("PlayerInput: JumpStart");
                }
            }
            
            if (context.canceled)
            {
                // При отпускании, если прыжок уже был инициирован, вызываем JumpEnd.
                if (jumpStarted)
                {
                    JumpEnd.OnNext(Unit.Default);
                    jumpStarted = false; // сброс флага после завершения прыжка
                    //Debug.Log("PlayerInput: JumpEnd");
                }
            }
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
