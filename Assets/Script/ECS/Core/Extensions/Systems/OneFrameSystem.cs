using Leopotam.EcsLite;

namespace ECS
{
    internal sealed class OneFrameSystem<T> : IEcsInitSystem, IEcsPostRunSystem where T : struct
    {
        private readonly EcsFilter _filter;
        private readonly EcsPool<T> _pool;

        public OneFrameSystem(EcsWorld world)
        {
            _filter = world.Filter<T>().End();
            _pool = world.GetPool<T>();
        }

        public void Init(IEcsSystems systems) => RemoveEvent();

        public void PostRun(IEcsSystems systems) => RemoveEvent();

        private void RemoveEvent()
        {
            foreach (var i in _filter)
            {
                _pool.Del(i);
            }
        }
    }
}