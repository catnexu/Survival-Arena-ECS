using Camera;
using ECS;
using Infrastructure;
using Input;
using UI;
using Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using Weapon;

namespace Core
{
    internal sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputModule;
        [SerializeField] private GameObject _interface;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private GameConfig _gameConfig;
        private ServiceLocator _locator;

        private void Start()
        {
            DontDestroyOnLoad(this);
            _locator = new ServiceLocator();
            _locator.Register<EventSystem>(_eventSystem);
            _locator.Register<InputSystemUIInputModule>(_inputModule);

            InfrastructureScope.Build(_locator);
            UnitScope.Build(_locator);
            InputScope.Build(_locator);
            CameraScope.Build(_locator, _camera);
            UIScope.Build(_locator, _interface);
            WeaponScope.Build(_locator);

            _locator.Register<IPlayerCreator, PlayerController>(new PlayerController());
            EcsScope.Build(_locator);
            StartGame();
        }

        private void StartGame()
        {
            GameController gameController = new GameController(_gameConfig, _locator.Resolve<IEcsService>(), _locator.Resolve<PlayerController>());
            _eventSystem.gameObject.SetActive(true);
            gameController.Start();
        }

        private void OnDestroy()
        {
            _locator?.Dispose();
        }
    }
}