using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class CoinInitAndCleanupSystem : IEcsRunSystem, IEcsPostRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<UnitComponent, TransformComponent, EnemyTag, UnitDeadComponent>, Exc<DestroyTag>> _deadFilter = default;
        private readonly EcsFilterInject<Inc<CoinComponent, DestroyTag>> _cleanupFilter = default;
        private readonly EcsFilterInject<Inc<CoinComponent>> _onDestroyFilter = default;
        private readonly EcsPoolInject<CoinComponent> _coinPool = default;
        private readonly ICoinsFactory _coinsFactory;

        public CoinInitAndCleanupSystem(ICoinsFactory coinsFactory)
        {
            _coinsFactory = coinsFactory;
        }

        public void Init(IEcsSystems systems)
        {
            _coinsFactory.OnCoinCreated += InitializeCoin;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _deadFilter.Value)
            {
                var position = _deadFilter.Pools.Inc2.Get(i).Value.position;
                _coinsFactory.RequestCoin(position);
            }
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var i in _cleanupFilter.Value)
            {
                _coinPool.Value.Get(i).View.Destroy();
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _coinsFactory.OnCoinCreated -= InitializeCoin;
            foreach (var i in _onDestroyFilter.Value)
            {
                _coinPool.Value.Get(i).View.Destroy();
                _world.Value.DelEntity(i);
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
    }
}