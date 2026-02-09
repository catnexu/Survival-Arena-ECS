using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class WeaponCleanupSystem : IEcsPostRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<WeaponComponent, DestroyTag>> _filter = default;
        private readonly IUnitWeaponMap _weaponMap;

        public WeaponCleanupSystem(IUnitWeaponMap weaponMap)
        {
            _weaponMap = weaponMap;
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var weaponComponent = ref _filter.Pools.Inc1.Get(entity);
                if (weaponComponent.Owner.Unpack(_world.Value, out var owner))
                {
                    _weaponMap.RemoveWeapon(owner, entity);
                }
            }
        }
    }
}