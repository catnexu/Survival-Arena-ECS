using System;

namespace Input
{
    public interface IInputService
    {
        IDisposable AddListener(IInputListener listener);
    }
}