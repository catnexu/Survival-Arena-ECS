using System;

namespace UI
{
    internal sealed class WindowViewModel : IStartWindowViewModel, IRestartWindowViewModel
    {
        public event Action<IViewModel> OnClose;
        private readonly Action _callback;

        public int Score { get; }

        public WindowViewModel(Action callback, int score = 0)
        {
            Score = score;
            _callback = callback;
        }

        public void Start()
        {
            _callback?.Invoke();
        }

        public void Close()
        {
            OnClose?.Invoke(this);
        }
    }
}