using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    internal sealed class UIController : InputMapController<IUIInputListener>
    {
        public UIController(IInputDevicesController inputController) : base(inputController)
        {
        }

        protected override void OnDispose(InputMap controller)
        {
            controller.UI.Navigate.performed -= OnNavigatePerformed;
            controller.UI.Navigate.canceled -= OnNavigateCanceled;
            controller.UI.Submit.performed -= OnSubmitPerformed;
            controller.UI.Submit.canceled -= OnSubmitCanceled;
            controller.UI.Cancel.performed -= OnCancel;
            controller.UI.ScrollWheel.performed -= OnScrollWheel;
            controller.UI.ScrollWheel.canceled -= OnScrollWheel;
        }

        protected override void Subscribe(InputMap controller)
        {
            controller.UI.Navigate.performed += OnNavigatePerformed;
            controller.UI.Navigate.canceled += OnNavigateCanceled;
            controller.UI.Submit.performed += OnSubmitPerformed;
            controller.UI.Submit.canceled += OnSubmitCanceled;
            controller.UI.Cancel.performed += OnCancel;
            controller.UI.ScrollWheel.performed += OnScrollWheel;
            controller.UI.ScrollWheel.canceled += OnScrollWheel;
        }

        protected override bool IsEnabled()
        {
            return Controller.UI.enabled;
        }

        protected override void OnEnable(InputMap controller)
        {
            controller.UI.Enable();
        }

        protected override void OnDisable(InputMap controller)
        {
            controller.UI.Disable();
        }

        private void OnNavigatePerformed(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].Navigate(value);
            }
        }

        private void OnNavigateCanceled(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].Navigate(Vector2.zero);
            }
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].StartSubmit();
            }
        }

        private void OnSubmitCanceled(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].CancelSubmit();
            }
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].Cancel();
            }
        }

        private void OnScrollWheel(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnScroll(value);
            }
        }
    }
}