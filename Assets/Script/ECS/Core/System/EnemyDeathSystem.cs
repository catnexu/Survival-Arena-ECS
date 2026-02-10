using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class EnemyDeathSystem : IEcsPostRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, UnitDeadComponent>, Exc<DestroyTag>> _filter = default;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly IUnitWeaponMap _weaponMap;

        public EnemyDeathSystem(IUnitWeaponMap weaponMap)
        {
            _weaponMap = weaponMap;
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref UnitComponent unitComponent = ref _filter.Pools.Inc1.Get(entity);
                if (_weaponMap.TryGetWeapons(entity, out IReadOnlyList<int> weapons))
                {
                    for (var i = 0; i < weapons.Count; i++)
                    {
                        var weapon = weapons[i];
                        if (_weaponPool.Value.Has(weapon))
                        {
                            _destroyPool.Value.Add(weapon);
                        }
                    }

                    _weaponMap.Clean(entity);
                }

                unitComponent.Value.Destroy();
                _destroyPool.Value.Add(entity);
            }
        }
    }
}