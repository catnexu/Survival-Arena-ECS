using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    internal sealed class PlayerController : InputMapController<IPlayerInputListener>
    {
        public PlayerController(IInputDevicesController inputController) : base(inputController)
        {
        }

        protected override void Subscribe(InputMap controller)
        {
            controller.Player.Move.performed += OnMove;
            controller.Player.Move.canceled += OnMove;
            controller.Player.Fire1.performed += OnFire1;
            controller.Player.Fire1.canceled += OnFire1;
            controller.Player.Fire2.performed += OnFire2;
            controller.Player.Fire2.canceled += OnFire2;
        }

        protected override void OnDispose(InputMap controller)
        {
            controller.Player.Move.performed -= OnMove;
            controller.Player.Move.canceled -= OnMove;
            controller.Player.Fire1.performed -= OnFire1;
            controller.Player.Fire1.canceled -= OnFire1;
            controller.Player.Fire2.performed -= OnFire2;
            controller.Player.Fire2.canceled -= OnFire2;
        }

        protected override bool IsEnabled()
        {
            return Controller.Player.enabled;
        }

        protected override void OnEnable(InputMap controller)
        {
            Controller.Player.Enable();
        }

        protected override void OnDisable(InputMap controller)
        {
            Controller.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].Move(value);
            }
        }
        
        private void OnFire1(InputAction.CallbackContext context) => OnFire(context, 0);
        private void OnFire2(InputAction.CallbackContext context) => OnFire(context, 1);


        private void OnFire(InputAction.CallbackContext context, int value)
        {
            bool isPerformed = context.performed;
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnFire(value, isPerformed);
            }
        }
    }
}