using Leopotam.EcsLite;

namespace ECS
{
    internal sealed class EntityCleanupSystem : IEcsPostRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;

        public EntityCleanupSystem(EcsWorld  world)
        {
            _world = world;
            _filter = world.Filter<DestroyTag>().End();
        }
        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _world.DelEntity(entity);
            }
        }
    }
}