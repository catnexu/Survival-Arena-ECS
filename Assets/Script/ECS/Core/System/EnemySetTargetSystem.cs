using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class EnemySetTargetSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, PlayerTag, TransformComponent>, Exc<DestroyTag>> _playerFilter = default;
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, TransformComponent, NavMeshComponent>, Exc<DestroyTag>> _enemyFilter = default;

        public void Run(IEcsSystems systems)
        {
            Vector3 playerPos = Vector3.zero;
            foreach (var entity in _playerFilter.Value)
            {
                ref TransformComponent playerTransform = ref _playerFilter.Pools.Inc3.Get(entity);
                playerPos = playerTransform.Value.position;
                break;
            }

            foreach (int entity in _enemyFilter.Value)
            {
                _enemyFilter.Pools.Inc4.Get(entity).Value.SetDestination(playerPos);
            }
        }
    }
}