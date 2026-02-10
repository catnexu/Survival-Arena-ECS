using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class HealthBarCleanupSystem : IEcsPostRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<HealthBarViewComponent, DestroyTag>> _filter = default;
        private readonly EcsFilterInject<Inc<HealthBarViewComponent>> _onDestroyFilter = default;
        
        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref HealthBarViewComponent component = ref _filter.Pools.Inc1.Get(entity);
                component.Value.Destroy();
            }
        }
        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _onDestroyFilter.Value)
            {
                ref HealthBarViewComponent component = ref _onDestroyFilter.Pools.Inc1.Get(entity);
                component.Value.Destroy();
            }
        }
    }
}