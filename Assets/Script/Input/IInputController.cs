using System;

namespace Input
{
    public interface IInputController : IDisposable
    {
        bool TryAddListener(IInputListener listener, out IDisposable subscription);
        void Enable();
        void Disable();
    }
}