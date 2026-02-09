using Leopotam.EcsLite;

namespace ECS
{
    internal struct DamageEvent
    {
        public EcsPackedEntity From;
        public EcsPackedEntity To;
        public float Damage;
    }
}