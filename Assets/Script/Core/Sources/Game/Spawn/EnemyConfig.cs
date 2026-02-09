using System;
using System.Collections.Generic;
using Navigation;
using Unit;
using UnityEngine;
using Weapon;

namespace Core
{
    [Serializable]
    internal struct EnemyConfig
    {
        [SerializeField, UnitId] private string _id;
        [SerializeField] private UnitStatsConfig _stats;
        [SerializeField, WeaponId] private string[] _weapons;
        [SerializeField] private NavMeshAgentConfiguration _agentConfig;

        public string Id => _id;
        public UnitStatsConfig Stats => _stats;
        public IReadOnlyList<string> Weapons => _weapons;
        public NavMeshAgentConfiguration AgentConfig => _agentConfig;
    }
}