using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class HealthBarUpdateSystem : IEcsRunSystem
    {
        private static readonly Quaternion s_healthBarRotation = Quaternion.Euler(90f, 0f, 0f);
        private readonly EcsFilterInject<Inc<UnitHealthComponent, HealthBarViewComponent, TransformComponent>, Exc<DestroyTag>> _filter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref UnitHealthComponent health = ref _filter.Pools.Inc1.Get(entity);
                ref HealthBarViewComponent healthBarView = ref _filter.Pools.Inc2.Get(entity);
                ref TransformComponent transform = ref _filter.Pools.Inc3.Get(entity);
                healthBarView.Value.UpdateHealth(health.Health, health.MaxHealth);
                Vector3 worldPosition = transform.Value.position + Vector3.forward;
                healthBarView.Value.Transform.SetPositionAndRotation(worldPosition, s_healthBarRotation);
            }
        }
    }
}