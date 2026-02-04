using Infrastructure;
using UnityEngine.InputSystem.UI;

namespace Input
{
    public static class InputScope
    {
        public static void Build(IServiceLocator locator)
        {
            InputSystemUIInputModule inputModule = locator.Resolve<InputSystemUIInputModule>();
            ITickController tickController = locator.Resolve<ITickController>();
            InputService inputService = new InputService(inputModule, tickController);
            locator.Register<IInputService>(inputService);
        }
    }
}