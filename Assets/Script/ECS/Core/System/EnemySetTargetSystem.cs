using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class EnemySetTargetSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, TransformComponent, NavMeshComponent>, Exc<DestroyTag>> _enemyFilter = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly EcsPoolInject<WeaponTargetComponent> _targetPool = default;
        private readonly IPlayerEntityProvider _playerEntityProvider;
        private readonly IUnitWeaponMap _weaponMap;
        public EnemySetTargetSystem(IPlayerEntityProvider playerEntityProvider, IUnitWeaponMap weaponMap)
        {
            _playerEntityProvider = playerEntityProvider;
            _weaponMap = weaponMap;
        }

        public void Run(IEcsSystems systems)
        {
            if(!_playerEntityProvider.TryGetNearestPlayer(Vector3.zero, out int _,  out var playerPos))
                return;

            foreach (int entity in _enemyFilter.Value)
            {
                _enemyFilter.Pools.Inc4.Get(entity).Value.SetDestination(playerPos);
                if (!_weaponMap.TryGetWeapons(entity, out IReadOnlyList<int> weapons) || weapons.Count == 0)
                    return;
                
                for (var index = 0; index < weapons.Count; index++)
                {
                    var weapon = weapons[index];
                    if (_weaponPool.Value.Has(weapon))
                    {
                        _targetPool.Value.Add(weapon).TargetPosition = playerPos;
                    }
                }
            }
        }
    }
}