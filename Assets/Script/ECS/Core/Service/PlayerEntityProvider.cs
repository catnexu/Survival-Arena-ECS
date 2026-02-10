using Leopotam.EcsLite;
using UnityEngine;

namespace ECS
{
    internal sealed class PlayerEntityProvider : IPlayerEntityProvider
    {
        private readonly EcsFilter _playerFilter;
        private readonly EcsPool<TransformComponent> _transformPool;

        public PlayerEntityProvider(EcsWorld world)
        {
            _playerFilter = world.Filter<UnitComponent>().Inc<TransformComponent>().Inc<PlayerTag>().Exc<DestroyTag>().End();
            _transformPool = world.GetPool<TransformComponent>();
        }

        public bool TryGetNearestPlayer(Vector3 origin, out int entity, out Vector3 position)
        {
            entity = -1;
            position = origin;
            //т.к. синглплеер отдаем просто первого
            foreach (var i in _playerFilter)
            {
                ref var transform = ref _transformPool.Get(i);
                position = transform.Value.position;
                return true;
            }

            return false;
        }
    }
}