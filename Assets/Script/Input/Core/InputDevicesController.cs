using UnityEngine.InputSystem.UI;

namespace Input
{
    internal sealed class InputDevicesController : IInputDevicesController
    {
        private readonly InputMap _map;

        public InputDevicesController(InputSystemUIInputModule inputModule)
        {
            _map = new InputMap();
            _map.Enable();
            inputModule.actionsAsset = _map.asset;
        }

        public InputMap GetMap() => _map;
    }
}