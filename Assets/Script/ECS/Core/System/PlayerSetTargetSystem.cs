using System.Collections.Generic;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class PlayerSetTargetSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, PlayerTag, TransformComponent>, Exc<DestroyTag>> _playerFilter = default;
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, TransformComponent>, Exc<DestroyTag>> _enemyFilter = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly EcsPoolInject<WeaponTargetComponent> _targetPool = default;
        private readonly IRandomizer _randomizer;
        private readonly IUnitWeaponMap _weaponMap;

        public PlayerSetTargetSystem(IRandomizer randomizer, IUnitWeaponMap weaponMap)
        {
            _randomizer = randomizer;
            _weaponMap = weaponMap;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int playerEntity in _playerFilter.Value)
            {
                if (!_weaponMap.TryGetWeapons(playerEntity, out IReadOnlyList<int> weapons) || weapons.Count == 0)
                    return;

                ref TransformComponent transform = ref _playerFilter.Pools.Inc3.Get(playerEntity);
                bool hasTarget = false;
                Vector3 ownerPos = transform.Value.position;
                Vector3 targetPos = ownerPos;
                float distanceSqr = float.MaxValue;
                foreach (var enemyEntity in _enemyFilter.Value)
                {
                    ref TransformComponent enemyTransform = ref _enemyFilter.Pools.Inc3.Get(enemyEntity);
                    Vector3 enemyPos = enemyTransform.Value.position;
                    var targetDistanceSqr = enemyPos.SqrDistance(ownerPos);
                    if (targetDistanceSqr < distanceSqr)
                    {
                        distanceSqr = targetDistanceSqr;
                        hasTarget = true;
                        targetPos = enemyPos;
                    }
                }

                if (!hasTarget)
                {
                    targetPos = new Vector3(ownerPos.x + _randomizer.GetRandom(-10f, 10f), ownerPos.y,
                        ownerPos.z + _randomizer.GetRandom(-10f, 10f));
                }


                for (var index = 0; index < weapons.Count; index++)
                {
                    var weapon = weapons[index];
                    if (_weaponPool.Value.Has(weapon))
                    {
                        _targetPool.Value.Add(weapon).TargetPosition = targetPos;
                    }
                }
            }
        }
    }
}