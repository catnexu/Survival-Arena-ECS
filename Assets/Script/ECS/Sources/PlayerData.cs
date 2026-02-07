using Unit;

namespace ECS
{
    public struct PlayerData
    {
        public readonly string Id;
        public readonly UnitStatsConfig Stats;

        public PlayerData(string id, UnitStatsConfig stats)
        {
            Id = id;
            Stats = stats;
        }
    }
}