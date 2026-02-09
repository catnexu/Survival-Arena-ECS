using Leopotam.EcsLite;

namespace ECS
{
    internal struct WeaponCreateEvent
    {
        public EcsPackedEntity Owner;
        public string Value;
    }
}