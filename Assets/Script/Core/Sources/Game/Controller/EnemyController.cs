using System;
using ECS;
using Infrastructure;
using Navigation;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    internal sealed class EnemyController : IEnemyFactory, IUpdate
    {
        private readonly IFieldViewProvider _fieldViewProvider;
        private readonly ITickController _tickController;
        private readonly IRandomizer _randomizer;
        private readonly INavigationGridService _navigationGridService;
        private IDisposable _updateSub;
        private bool _started;
        private EnemySpawnConfig _config;
        private float _tick;
        public event Action<EnemyData> OnNewUnitEvent;

        public EnemyController(IFieldViewProvider fieldViewProvider,
            ITickController tickController,
            IRandomizer randomizer,
            INavigationGridService navigationGridService)
        {
            _fieldViewProvider = fieldViewProvider;
            _tickController = tickController;
            _randomizer = randomizer;
            _navigationGridService = navigationGridService;
        }

        public void Start(EnemySpawnConfig config)
        {
            if (_started)
                return;
            _started = true;
            _config = config;
            _updateSub = _tickController.AddController(this);
            _tick = _config.SpawnCooldown;
        }

        public void Stop()
        {
            if (!_started)
                return;
            _started = false;
            _updateSub?.Dispose();
        }

        public void UpdateController(float deltaTime)
        {
            _tick -= deltaTime;
            if (_tick <= 0)
            {
                _tick += _config.SpawnCooldown;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            EnemyConfig enemy = _config.Enemies[_randomizer.GetRandom(0, _config.Enemies.Count)];
            Vector3 bottomLeft = _fieldViewProvider.BottomLeft;
            Vector3 topRight = _fieldViewProvider.TopRight;
            Vector3 spawnPos = GeneratePosition(bottomLeft, topRight);
            OnNewUnitEvent?.Invoke(new EnemyData(enemy.Id, enemy.Stats, enemy.Weapons, enemy.AgentConfig, spawnPos));
        }

        private Vector3 GeneratePosition(Vector3 bottomLeft, Vector3 topRight)
        {
            int side = _randomizer.GetRandom(0, 4);
            var outOfViewPosition = side switch
            {
                0 => new Vector3(bottomLeft.x - _config.ViewSpawnMargin - _randomizer.GetRandom(0f, _config.ViewSpawnExtraMargin),
                    0f, _randomizer.GetRandom(bottomLeft.z - _config.ViewSpawnExtraMargin, topRight.z + _config.ViewSpawnExtraMargin)),
                1 => new Vector3(topRight.x + _config.ViewSpawnMargin + _randomizer.GetRandom(0f, _config.ViewSpawnExtraMargin),
                    0f, _randomizer.GetRandom(bottomLeft.z - _config.ViewSpawnExtraMargin, topRight.z + _config.ViewSpawnExtraMargin)),
                2 => new Vector3(_randomizer.GetRandom(bottomLeft.x - _config.ViewSpawnExtraMargin, topRight.x + _config.ViewSpawnExtraMargin),
                    0f, bottomLeft.z - _config.ViewSpawnMargin - _randomizer.GetRandom(0f, _config.ViewSpawnExtraMargin)),
                _ => new Vector3(_randomizer.GetRandom(bottomLeft.x - _config.ViewSpawnExtraMargin, topRight.x + _config.ViewSpawnExtraMargin),
                    0f, topRight.z + _config.ViewSpawnMargin + _randomizer.GetRandom(0f, _config.ViewSpawnExtraMargin))
            };

            return _navigationGridService.GetRandomPositionInRadius(NavigationGridType.Humanoid, outOfViewPosition, _config.SpawnRadius);
        }
    }
}