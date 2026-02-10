using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class CoinStorageSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<CoinPickEvent>> _eventFilter = WorldNames.EVENT;
        private readonly EcsPoolInject<UnitCoinStorageComponent> _storagePool = default;
        public CoinStorageSystem()
        {
            
        }
        public void Run(IEcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref CoinPickEvent eventComponent = ref _eventFilter.Pools.Inc1.Get(i);
                if (eventComponent.UnitEntity.Unpack(_world.Value, out var entity) && _storagePool.Value.Has(entity))
                {
                    ref UnitCoinStorageComponent storageComponent = ref _storagePool.Value.Get(entity);
                    storageComponent.Value += eventComponent.Value;
                }
            }
        }
    }
}