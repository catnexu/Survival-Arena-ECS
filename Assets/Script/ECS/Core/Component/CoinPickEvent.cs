using Leopotam.EcsLite;

namespace ECS
{
    internal struct CoinPickEvent
    {
        public EcsPackedEntity UnitEntity;
        public int Value;
    }
}