using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class ProjectileCleanupSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<ProjectileViewComponent, DestroyTag>> _filter = default;
        private readonly EcsFilterInject<Inc<ProjectileViewComponent>> _onDestroyFilter = default;
        private readonly IPoolService _poolService;
        
        public ProjectileCleanupSystem(IPoolService poolService)
        {
            _poolService = poolService;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref ProjectileViewComponent view = ref _filter.Pools.Inc1.Get(entity);
                view.View.Destroy();
                _poolService.Destroy(view.View.gameObject);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _onDestroyFilter.Value)
            {
                ref ProjectileViewComponent view = ref _onDestroyFilter.Pools.Inc1.Get(entity);
                view.View.Destroy();
                _poolService.Destroy(view.View.gameObject);
            }
        }
    }
}