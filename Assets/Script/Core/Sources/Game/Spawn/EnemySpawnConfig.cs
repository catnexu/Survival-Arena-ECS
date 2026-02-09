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

        public IReadOnlyList<EnemyConfig> Enemies => _enemies;
        public float SpawnCooldown => _spawnCooldown;
    }
}