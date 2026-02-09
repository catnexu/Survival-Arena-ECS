using Leopotam.EcsLite;

namespace ECS
{
    internal struct WeaponComponent
    {
        public EcsPackedEntity Owner;
        public float ReloadTime;
        public float TimeSinceLastShot;
        public bool IsCharged;
        public bool IsActive;
        public int TargetLayer;
    }
}