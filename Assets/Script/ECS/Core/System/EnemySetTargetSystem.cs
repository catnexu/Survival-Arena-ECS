using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class EnemySetTargetSystem : IEcsRunSystem
    {
        private readonly IPlayerEntityProvider _playerEntityProvider;
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, TransformComponent, NavMeshComponent>, Exc<DestroyTag>> _enemyFilter = default;

        public EnemySetTargetSystem(IPlayerEntityProvider playerEntityProvider)
        {
            _playerEntityProvider = playerEntityProvider;
        }

        public void Run(IEcsSystems systems)
        {
            if(!_playerEntityProvider.TryGetNearestPlayer(Vector3.zero, out var playerEntity,  out var playerPos))
                return;

            foreach (int entity in _enemyFilter.Value)
            {
                _enemyFilter.Pools.Inc4.Get(entity).Value.SetDestination(playerPos);
            }
        }
    }
}