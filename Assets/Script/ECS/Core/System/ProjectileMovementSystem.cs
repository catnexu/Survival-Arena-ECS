using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class ProjectileMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ProjectileComponent, ProjectileViewComponent>, Exc<DestroyTag>> _filter = default;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = default;
        private readonly ITimeManager _timeManager;

        public ProjectileMovementSystem(ITimeManager timeManager)
        {
            _timeManager = timeManager;
        }
        public void Run(IEcsSystems systems)
        {
            float deltaTime = _timeManager.DeltaTime;
            
            foreach (var entity in _filter.Value)
            {
                ref var projectile = ref _filter.Pools.Inc1.Get(entity);
                ref var view = ref _filter.Pools.Inc2.Get(entity);
                
                view.Transform.position += projectile.Direction * (projectile.Speed * deltaTime);
                projectile.LifeTime -= deltaTime;
                
                if (projectile.LifeTime <= 0f)
                {
                    _destroyPool.Value.Add(entity);
                }
            }
        }
    }
}