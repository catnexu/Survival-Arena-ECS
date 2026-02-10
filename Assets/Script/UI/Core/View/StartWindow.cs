using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class StartWindow : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        private IStartWindowViewModel _viewModel;
        
        public void Initialize(IStartWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            _startButton.onClick.AddListener(StartGame);
            viewModel.OnClose += CloseView;
            gameObject.SetActive(true);
        }

        private void StartGame()
        {
            _viewModel.Start();
        }

        private void CloseView(IViewModel viewModel)
        {
            _viewModel.OnClose -= CloseView;
            _startButton.onClick.RemoveListener(StartGame);
            _viewModel = null;
            gameObject.SetActive(false);
        }
    }
}