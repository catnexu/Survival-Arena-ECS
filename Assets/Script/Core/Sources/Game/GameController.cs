using ECS;

namespace Core
{
    internal sealed class GameController
    {
        private readonly GameConfig _gameConfig;
        private readonly IEcsService _ecsService;
        private readonly PlayerController _playerController;

        public GameController(GameConfig gameConfig, IEcsService ecsService, PlayerController playerController)
        {
            _gameConfig = gameConfig;
            _ecsService = ecsService;
            _playerController = playerController;
        }

        public void Start()
        {
            IEcsController controller = _ecsService.CreateController();
            controller.Start();
            _playerController.Start(_gameConfig.PlayerUnit);
        }
    }
}