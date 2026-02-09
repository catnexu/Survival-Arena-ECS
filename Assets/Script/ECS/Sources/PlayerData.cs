using System.Collections.Generic;
using Unit;

namespace ECS
{
    public struct PlayerData
    {
        public readonly string Id;
        public readonly UnitStatsConfig Stats;
        public readonly IReadOnlyList<string> Weapons;

        public PlayerData(string id, UnitStatsConfig stats, IReadOnlyList<string> weapons)
        {
            Id = id;
            Stats = stats;
            Weapons = weapons;
        }
    }
}