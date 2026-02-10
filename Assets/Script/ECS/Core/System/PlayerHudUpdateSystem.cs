using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class PlayerHudUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, UnitHealthComponent, PlayerHudViewComponent>> _playerFilter = default;
        private readonly EcsFilterInject<Inc<PlayerTag, UnitCoinStorageComponent, PlayerHudViewComponent>> _coinFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                ref UnitHealthComponent health = ref _playerFilter.Pools.Inc2.Get(entity);
                ref PlayerHudViewComponent hud = ref _playerFilter.Pools.Inc3.Get(entity);
                hud.Value.UpdateHealth(health.Health, health.MaxHealth);
            }

            foreach (var entity in _coinFilter.Value)
            {
                ref UnitCoinStorageComponent coins = ref _coinFilter.Pools.Inc2.Get(entity);
                ref PlayerHudViewComponent hud = ref _playerFilter.Pools.Inc3.Get(entity);
                hud.Value.UpdateCoins(coins.Value);
            }
        }
    }
}