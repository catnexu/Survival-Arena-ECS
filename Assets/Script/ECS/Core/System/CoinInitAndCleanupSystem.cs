using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class CoinInitAndCleanupSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<UnitComponent, TransformComponent, EnemyTag, UnitDeadComponent>, Exc<DestroyTag>> _deadFilter = default;
        private readonly EcsFilterInject<Inc<CoinComponent>> _cleanupFilter = default;
        private readonly EcsPoolInject<CoinComponent> _coinPool = default;
        private readonly ICoinsCreator _coinsCreator;

        public CoinInitAndCleanupSystem(ICoinsCreator coinsCreator)
        {
            _coinsCreator = coinsCreator;
        }

        public void Init(IEcsSystems systems)
        {
            _coinsCreator.OnCoinCreated += InitializeCoin;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _deadFilter.Value)
            {
                var position = _deadFilter.Pools.Inc2.Get(i).Value.position;
                _coinsCreator.RequestCoin(position);
            }
        }

        private void InitializeCoin(ICoin coin, int value, float pickRange)
        {
            var entity = _world.Value.NewEntity();
            ref CoinComponent coinComponent = ref _coinPool.Value.Add(entity);
            coinComponent.View = coin;
            coinComponent.Value = value;
            coinComponent.PickRangeSqr = pickRange;
        }

        public void Destroy(IEcsSystems systems)
        {
            _coinsCreator.OnCoinCreated -= InitializeCoin;
            foreach (var i in _cleanupFilter.Value)
            {
                _coinPool.Value.Get(i).View.Destroy();
                _world.Value.DelEntity(i);
            }
        }
    }
}