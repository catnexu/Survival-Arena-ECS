using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class PlayerDeathSystem : IEcsPostRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, InputMoveComponent, UnitDeadComponent>, Exc<DestroyTag>> _filter = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly EcsPoolInject<UnitCoinStorageComponent> _storagePool = default;
        private readonly IUnitWeaponMap _weaponMap;
        private readonly IGameEventsProvider _eventsProvider;

        public PlayerDeathSystem(IUnitWeaponMap weaponMap, IGameEventsProvider eventsProvider)
        {
            _weaponMap = weaponMap;
            _eventsProvider = eventsProvider;
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _filter.Pools.Inc2.Del(entity);
                if (_weaponMap.TryGetWeapons(entity, out IReadOnlyList<int> weapons))
                {
                    for (var i = 0; i < weapons.Count; i++)
                    {
                        var weapon = weapons[i];
                        if (_weaponPool.Value.Has(weapon))
                        {
                            _weaponPool.Value.Get(weapon).IsActive = false;
                        }
                    }
                }

                int score = 0;
                if (_storagePool.Value.Has(entity))
                {
                    score = _storagePool.Value.Get(entity).Value;
                }

                _eventsProvider.GameOver(score);
            }
        }
    }
}