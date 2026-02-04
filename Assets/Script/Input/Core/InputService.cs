using System;
using Infrastructure;
using R3;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Input
{
    internal sealed class InputService : IInputService, IUnscaledUpdate, IDisposable
    {
        private readonly IInputController[] _controllers;
        private readonly IDisposable _updateSub;

        public InputService(InputSystemUIInputModule inputModule, ITickController tickController)
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;
            InputDevicesController mapController = new InputDevicesController(inputModule);
            _controllers = new IInputController[] {new UIController(mapController), new PlayerController(mapController)};
            _updateSub = tickController.AddController(this);
        }

        public void Dispose()
        {
            _updateSub?.Dispose();
            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i].Dispose();
            }
        }

        public IDisposable AddListener(IInputListener listener)
        {
            return TryAddListener(listener);
        }

        private IDisposable TryAddListener(IInputListener listener)
        {
            CompositeDisposable compositeDisposable = new CompositeDisposable();
            for (int i = 0; i < _controllers.Length; i++)
            {
                if (_controllers[i].TryAddListener(listener, out IDisposable sub))
                {
                    compositeDisposable.Add(sub);
                }
            }

            return compositeDisposable;
        }

        void IUnscaledUpdate.UpdateController(float deltaTime) => InputSystem.Update();
    }
}