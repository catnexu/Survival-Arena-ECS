using ECS;
using UI;

namespace Core
{
    internal sealed class GameController
    {
        private readonly GameConfig _gameConfig;
        private readonly IEcsService _ecsService;
        private readonly PlayerController _playerController;
        private readonly EnemyController _enemyController;
        private readonly CoinsController _coinsController;
        private readonly MenuPresenter _presenter;
        private IEcsController _currentController;

        public GameController(GameConfig gameConfig,
            IEcsService ecsService,
            PlayerController playerController,
            EnemyController enemyController,
            CoinsController coinsController,
            MenuPresenter presenter)
        {
            _gameConfig = gameConfig;
            _ecsService = ecsService;
            _playerController = playerController;
            _enemyController = enemyController;
            _coinsController = coinsController;
            _presenter = presenter;
        }

        public void Start()
        {
            _presenter.ShowStart(StartGame);
        }

        private void StartGame()
        {
            _presenter.Hide();
            _currentController = _ecsService.CreateController();
            _currentController.Start();
            _playerController.OnGameOver += OnGameOver;
            _playerController.Start(_gameConfig.Player);
            _enemyController.Start(_gameConfig.EnemySpawn);
            _coinsController.Start(_gameConfig.Coins);
        }

        private void OnGameOver(int score)
        {
            _playerController.OnGameOver -= OnGameOver;
            _enemyController.Stop();
            _coinsController.Stop();
            _currentController?.Dispose();
            _currentController = null;
            _presenter.ShowRestart(StartGame, score);
        }
    }
}