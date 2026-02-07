using Infrastructure;
using Leopotam.EcsLite;

namespace ECS
{
    internal sealed class DelayTimeRemoveSystem<T> : IEcsInitSystem, IEcsPostRunSystem where T : struct
    {
        private struct DelayTimeRemoveComponent
        {
            public float Time;
            public EcsPackedEntity Entity;
        }

        private readonly ITimeManager _timeService;
        private readonly EcsWorld _world;
        private readonly EcsFilter _initFilter;
        private readonly EcsFilter _filter;
        private readonly EcsPool<DelayTimeRemoveInitComponent<T>> _initTimePool;
        private readonly EcsPool<DelayTimeRemoveComponent> _timePool;
        private readonly EcsPool<T> _componentPool;

        public DelayTimeRemoveSystem(ITimeManager timeService, EcsWorld world)
        {
            _timeService = timeService;
            _world = world;
            _initFilter = world.Filter<DelayTimeRemoveInitComponent<T>>().Exc<DelayTimeRemoveComponent>().End();
            _filter = world.Filter<DelayTimeRemoveComponent>().End();
            _initTimePool = world.GetPool<DelayTimeRemoveInitComponent<T>>();
            _timePool = world.GetPool<DelayTimeRemoveComponent>();
            _componentPool = world.GetPool<T>();
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
                    _componentPool.TryDel(entity);
                }

                _world.DelEntity(i);
            }
        }
    }
}