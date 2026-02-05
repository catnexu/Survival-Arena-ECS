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
        }

        protected override void OnDispose(InputMap controller)
        {
            controller.Player.Move.performed -= OnMove;
            controller.Player.Move.canceled -= OnMove;
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
    }
}