using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class WeaponCleanupSystem : IEcsPostRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<WeaponComponent, WeaponMuzzleComponent, DestroyTag>> _filter = default;
        private readonly EcsFilterInject<Inc<WeaponComponent, WeaponMuzzleComponent>> _onDestroyFilter = default;
        private readonly IUnitWeaponMap _weaponMap;
        private readonly IPoolService _poolService;

        public WeaponCleanupSystem(IUnitWeaponMap weaponMap, IPoolService poolService)
        {
            _weaponMap = weaponMap;
            _poolService = poolService;
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref WeaponComponent weaponComponent = ref _filter.Pools.Inc1.Get(entity);
                ref WeaponMuzzleComponent muzzle = ref _filter.Pools.Inc2.Get(entity);
                _poolService.Destroy(muzzle.Value.gameObject);
                _filter.Pools.Inc2.Del(entity);
                if (weaponComponent.Owner.Unpack(_world.Value, out var owner))
                {
                    _weaponMap.RemoveWeapon(owner, entity);
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _onDestroyFilter.Value)
            {
                ref WeaponMuzzleComponent muzzle = ref _onDestroyFilter.Pools.Inc2.Get(entity);
                _poolService.Destroy(muzzle.Value.gameObject);
            }
        }
    }
}