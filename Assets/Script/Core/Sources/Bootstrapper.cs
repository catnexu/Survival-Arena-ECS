using Infrastructure;
using Input;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Core
{
    internal sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputModule;
        [SerializeField] private GameObject _interface;
        private ServiceLocator _locator;

        private void Start()
        {
            DontDestroyOnLoad(this);
            _locator = new ServiceLocator();
            _locator.Register<EventSystem>(_eventSystem);
            _locator.Register<InputSystemUIInputModule>(_inputModule);
            InfrastructureScope.Build(_locator);
            UIScope.Build(_locator, _interface);
            InputScope.Build(_locator);
            StartGame();
        }

        private void StartGame()
        {
            GameController gameController = new GameController();
            _eventSystem.gameObject.SetActive(true);
            gameController.Start();
        }

        private void OnDestroy()
        {
            _locator?.Dispose();
        }
    }
}