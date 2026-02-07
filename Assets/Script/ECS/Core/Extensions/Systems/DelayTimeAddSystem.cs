using Infrastructure;
using Leopotam.EcsLite;

namespace ECS
{
    internal sealed class DelayTimeAddSystem<T> : IEcsInitSystem, IEcsPostRunSystem where T : struct
    {
        private struct DelayTimeAddComponent
        {
            public float Time;
            public EcsPackedEntity Entity;
        }

        private readonly EcsWorld _world;
        private readonly EcsFilter _initFilter;
        private readonly EcsFilter _filter;
        private readonly EcsPool<DelayTimeAddInitComponent<T>> _initTimePool;
        private readonly EcsPool<DelayTimeAddComponent> _timePool;
        private readonly EcsPool<T> _componentPool;
        private readonly ITimeManager _timeService;

        public DelayTimeAddSystem(ITimeManager timeService, EcsWorld world)
        {
            _world = world;
            _initFilter = world.Filter<DelayTimeAddInitComponent<T>>().Exc<DelayTimeAddComponent>().End();
            _filter = world.Filter<DelayTimeAddComponent>().End();
            _initTimePool = world.GetPool<DelayTimeAddInitComponent<T>>();
            _timePool = world.GetPool<DelayTimeAddComponent>();
            _componentPool = world.GetPool<T>();
            _timeService = timeService;
        }

        public void Init(IEcsSystems systems) => CheckTimer();

        public void PostRun(IEcsSystems systems) => CheckTimer();

        private void CheckTimer()
        {
            float currentTime = _timeService.CurrentTime;
            foreach (var i in _initFilter)
            {
                ref var timeEntity = ref _initTimePool.Get(i);
                ref var delayTimeAddComponent = ref _timePool.Add(i);
                delayTimeAddComponent.Time = currentTime + timeEntity.DelaySec;
                delayTimeAddComponent.Entity = timeEntity.Entity;
                _initTimePool.Del(i);
            }

            foreach (var i in _filter)
            {
                ref var timeEntity = ref _timePool.Get(i);
                if (timeEntity.Time > currentTime)
                    continue;

                if (timeEntity.Entity.Unpack(_world, out int entity))
                {
                    _componentPool.GetOrAdd(entity);
                }

                _world.DelEntity(i);
            }
        }
    }
}