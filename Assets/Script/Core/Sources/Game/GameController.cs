using ECS;

namespace Core
{
    internal sealed class GameController
    {
        private readonly GameConfig _gameConfig;
        private readonly IEcsService _ecsService;
        private readonly PlayerController _playerController;
        private readonly EnemyController _enemyController;

        public GameController(GameConfig gameConfig, IEcsService ecsService, PlayerController playerController, EnemyController enemyController)
        {
            _gameConfig = gameConfig;
            _ecsService = ecsService;
            _playerController = playerController;
            _enemyController = enemyController;
        }

        public void Start()
        {
            IEcsController controller = _ecsService.CreateController();
            controller.Start();
            _playerController.Start(_gameConfig.Player);
            _enemyController.Start(_gameConfig.EnemySpawn);
        }
    }
}