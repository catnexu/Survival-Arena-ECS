using System;
using ECS;
using Infrastructure;
using Navigation;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    internal sealed class EnemyController : IEnemyCreator, IUpdate
    {
        private const float Margin = 2f;
        private const float ExtraMargin = 4f;
        private const int MaxTryCount = 4;
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

        public void UpdateController(float deltaTime)
        {
            _tick -= deltaTime;
            if (_tick <= 0)
            {
                _tick = _config.SpawnCooldown;
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
                0 => new Vector3(bottomLeft.x - Margin - _randomizer.GetRandom(0f, ExtraMargin),
                    0f, _randomizer.GetRandom(bottomLeft.z - ExtraMargin, topRight.z + ExtraMargin)),
                1 => new Vector3(topRight.x + Margin + _randomizer.GetRandom(0f, ExtraMargin),
                    0f, _randomizer.GetRandom(bottomLeft.z - ExtraMargin, topRight.z + ExtraMargin)),
                2 => new Vector3(_randomizer.GetRandom(bottomLeft.x - ExtraMargin, topRight.x + ExtraMargin),
                    0f, bottomLeft.z - Margin - _randomizer.GetRandom(0f, ExtraMargin)),
                _ => new Vector3(_randomizer.GetRandom(bottomLeft.x - ExtraMargin, topRight.x + ExtraMargin),
                    0f, topRight.z + Margin + _randomizer.GetRandom(0f, ExtraMargin))
            };

            return _navigationGridService.GetRandomPositionInRadius(NavigationGridType.Humanoid, outOfViewPosition, 10f);
        }
    }
}