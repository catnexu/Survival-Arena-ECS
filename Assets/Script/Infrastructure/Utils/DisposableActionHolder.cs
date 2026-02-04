using System;

namespace Infrastructure
{
    public sealed class DisposableActionHolder : IDisposable
    {
        private Action _disposeAction;

        public DisposableActionHolder(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            if (_disposeAction != null)
            {
                Action temp = _disposeAction;
                _disposeAction = null;
                temp.Invoke();
            }
        }
    }
}