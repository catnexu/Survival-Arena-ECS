using System;

namespace Infrastructure
{
    public interface ITickController
    {
        ITimeManager TimeManager { get; }
        IDisposable AddController(IController controller);
    }
}