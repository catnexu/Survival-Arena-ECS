using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [Serializable]
    internal struct EnemySpawnConfig
    {
        [SerializeField] private EnemyConfig[] _enemies;
        [SerializeField] private float _spawnCooldown;
        [SerializeField] private float _viewSpawnMargin;
        [SerializeField] private float _viewSpawnExtraMargin;
        [SerializeField] private float spawnRadius;
        
        public IReadOnlyList<EnemyConfig> Enemies => _enemies;
        public float SpawnCooldown => _spawnCooldown;
        public float ViewSpawnMargin => _viewSpawnMargin;
        public float ViewSpawnExtraMargin => _viewSpawnExtraMargin;
        public float SpawnRadius => spawnRadius;
    }
}