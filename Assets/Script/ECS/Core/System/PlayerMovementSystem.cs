using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class PlayerMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, InputMoveComponent, RigidbodyComponent, UnitStatsComponent>> _filter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var input = ref _filter.Pools.Inc2.Get(entity);
                ref var rb = ref _filter.Pools.Inc3.Get(entity);
                ref var stats = ref _filter.Pools.Inc4.Get(entity);
                Vector3 moveDirection = new Vector3(input.Value.x, 0f, input.Value.y).normalized;
                Vector3 targetVelocity = moveDirection * stats.Speed;
                targetVelocity.y = rb.Value.linearVelocity.y;
                rb.Value.linearVelocity = targetVelocity;
            }
        }
    }
}