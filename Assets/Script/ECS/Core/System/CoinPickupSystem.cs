using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class CoinPickupSystem : IEcsRunSystem
    {
        private readonly IPlayerEntityProvider _playerProvider;
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;
        private readonly EcsFilterInject<Inc<CoinComponent>, Exc<DestroyTag>> _filter = default;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = default;

        public CoinPickupSystem(IPlayerEntityProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter.Value)
            {
                ref CoinComponent coinComponent = ref _filter.Pools.Inc1.Get(i);
                var coinPosition = coinComponent.View.Transform.position;
                if (_playerProvider.TryGetNearestPlayer(coinPosition, out int entity, out Vector3 position))
                {
                    if (coinPosition.SqrDistance(position) <= coinComponent.PickRangeSqr)
                    {
                        ref CoinPickEvent coinPickEvent = ref _eventWorld.Value.SendEvent<CoinPickEvent>();
                        coinPickEvent.UnitEntity = _world.Value.PackEntity(entity);
                        coinPickEvent.Value = coinComponent.Value;
                        _destroyPool.Value.Add(i);
                    }
                }
            }
        }
    }
}