using System;

namespace UI
{
    public sealed class MenuPresenter
    {
        private readonly StartWindow _startWindow;
        private readonly RestartWindow _restartWindow;
        private IViewModel _currentViewModel;

        internal MenuPresenter(StartWindow startWindow, RestartWindow restartWindow)
        {
            _startWindow = startWindow;
            _restartWindow = restartWindow;
        }

        public void ShowStart(Action callback)
        {
            WindowViewModel viewModel = new WindowViewModel(callback);
            _currentViewModel = viewModel;
            _startWindow.Initialize(viewModel);
        }
        
        public void ShowRestart(Action callback, int score)
        {
            WindowViewModel viewModel = new WindowViewModel(callback, score);
            _currentViewModel = viewModel;
            _restartWindow.Initialize(viewModel);
        }
        
        public void Hide()
        {
            _currentViewModel?.Close();
            _currentViewModel = null;
        }
    }
}