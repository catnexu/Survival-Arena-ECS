using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class RestartWindow : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _score;
        private IRestartWindowViewModel _viewModel;
        
        public void Initialize(IRestartWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            _startButton.onClick.AddListener(StartGame);
            _score.text = $"{viewModel.Score}";
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