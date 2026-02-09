using System.Collections.Generic;
using Unit;

namespace ECS
{
    public sealed class PlayerData
    {
        public readonly string Id;
        public readonly UnitStatsConfig Stats;
        public readonly IReadOnlyList<string> Weapons;
        public readonly float Speed;


        public PlayerData(string id, UnitStatsConfig stats, IReadOnlyList<string> weapons, float speed)
        {
            Id = id;
            Stats = stats;
            Weapons = weapons;
            Speed = speed;
        }
    }
}