using System.Collections.Generic;
using Navigation;
using Unit;
using UnityEngine;

namespace ECS
{
    public sealed class EnemyData
    {
        public readonly string Id;
        public readonly UnitStatsConfig Stats;
        public readonly IReadOnlyList<string> Weapons;
        public readonly NavMeshAgentConfiguration NavMeshAgentConfiguration;
        public readonly Vector3 Position;

        public EnemyData(string id,
            UnitStatsConfig stats,
            IReadOnlyList<string> weapons,
            NavMeshAgentConfiguration navMeshAgentConfiguration,
            Vector3 position)
        {
            Id = id;
            Stats = stats;
            Weapons = weapons;
            Position = position;
            NavMeshAgentConfiguration = navMeshAgentConfiguration;
        }
    }
}