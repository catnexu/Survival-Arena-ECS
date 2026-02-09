using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class ProjectileCleanupSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ProjectileViewComponent, DestroyTag>> _filter = default;
        private readonly IPoolService _poolService;
        
        public ProjectileCleanupSystem(IPoolService poolService)
        {
            _poolService = poolService;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var view = ref _filter.Pools.Inc1.Get(entity);
                view.View.Destroy();
                _poolService.Destroy(view.View.gameObject);
            }
        }
    }
}