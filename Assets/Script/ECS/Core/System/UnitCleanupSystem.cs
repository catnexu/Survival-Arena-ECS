using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class UnitCleanupSystem : IEcsPostRunSystem, IEcsDestroySystem
    {
        private readonly IUnitWeaponMap _weaponMap;
        private readonly EcsFilterInject<Inc<UnitComponent, DestroyTag>> _filter = default;
        private readonly EcsFilterInject<Inc<UnitComponent>> _onDestroyFilter = default;

        public UnitCleanupSystem(IUnitWeaponMap weaponMap)
        {
            _weaponMap = weaponMap;
        }
        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref UnitComponent component = ref _filter.Pools.Inc1.Get(entity);
                component.Value.Destroy();
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _onDestroyFilter.Value)
            {
                ref UnitComponent component = ref _onDestroyFilter.Pools.Inc1.Get(entity);
                component.Value.Destroy();
            }
        }
    }
}