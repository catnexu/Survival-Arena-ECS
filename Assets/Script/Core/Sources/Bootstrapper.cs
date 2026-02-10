using Camera;
using ECS;
using Infrastructure;
using Input;
using Navigation;
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
            NavigationScope.Build(_locator);

            _locator.Register<IPlayerFactory, PlayerController>(new PlayerController());
            _locator.Register<ICoinsFactory, CoinsController>(new CoinsController(_locator.Resolve<IPoolService>(), _locator.Resolve<IRandomizer>(),
                _locator.Resolve<INavigationGridService>()));
            _locator.Register<IEnemyFactory, EnemyController>(new EnemyController(_locator.Resolve<IFieldViewProvider>(),
                _locator.Resolve<ITickController>(), _locator.Resolve<IRandomizer>(), _locator.Resolve<INavigationGridService>()));
            EcsScope.Build(_locator);
            StartGame();
        }

        private void StartGame()
        {
            GameController gameController = new GameController(_gameConfig, _locator.Resolve<IEcsService>(), _locator.Resolve<PlayerController>(),
                _locator.Resolve<EnemyController>(), _locator.Resolve<CoinsController>());
            _eventSystem.gameObject.SetActive(true);
            gameController.Start();
        }

        private void OnDestroy()
        {
            _locator?.Dispose();
        }
    }
}