using System;
using System.Collections.Generic;
using Infrastructure;

namespace Input
{
    internal abstract class InputMapController<T> : IInputController where T : IInputListener
    {
        private bool _enabled;
        protected readonly List<T> Listeners;
        protected InputMap Controller { get; }

        protected InputMapController(IInputDevicesController inputController)
        {
            Controller = inputController.GetMap();
            Listeners = new List<T>(4);
            Subscribe(Controller);
            _enabled = IsEnabled();
        }

        protected abstract void Subscribe(InputMap controller);

        public virtual void Dispose()
        {
            Listeners.Clear();
            OnDispose(Controller);
        }

        public void Enable()
        {
            if (!_enabled)
            {
                _enabled = true;
                OnEnable(Controller);
            }
        }

        public void Disable()
        {
            if (_enabled)
            {
                _enabled = false;
                OnDisable(Controller);
            }
        }

        public bool TryAddListener(IInputListener listener, out IDisposable subscription)
        {
            if (listener is T targetListener)
            {
                Listeners.Add(targetListener);
                OnAddListener(targetListener);
                subscription = new DisposableActionHolder(() => RemoveListener(targetListener));
                return true;
            }

            subscription = null;
            return false;
        }

        protected virtual void OnAddListener(T listener)
        {
        }

        protected abstract bool IsEnabled();

        protected abstract void OnDispose(InputMap controller);

        protected abstract void OnEnable(InputMap controller);

        protected abstract void OnDisable(InputMap controller);

        private void RemoveListener(T listener) => Listeners.Remove(listener);
    }
}