using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class WeaponDamageSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;
        private readonly EcsFilterInject<Inc<DamageEvent>, Exc<DestroyTag>> _eventFilter = WorldNames.EVENT;
        private readonly EcsFilterInject<Inc<DamageEvent, DestroyTag>> _cleanupFilter = WorldNames.EVENT;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = WorldNames.EVENT;
        private readonly EcsPoolInject<UnitDeadComponent> _deadPool = default;
        private readonly EcsPoolInject<UnitHealthComponent> _healthPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref var damageEvent = ref _eventFilter.Pools.Inc1.Get(i);
                if (damageEvent.From.Unpack(_world.Value, out var fromEntity) && damageEvent.To.Unpack(_world.Value, out var toEntity))
                {
                    if (_deadPool.Value.Has(toEntity))
                        continue;
                    ref UnitHealthComponent healthComponent = ref _healthPool.Value.TryGetRef(toEntity, out var success);
                    healthComponent.Health -= damageEvent.Damage;
                    if (healthComponent.Health <= 0)
                    {
                        _deadPool.Value.Add(toEntity).Killer = damageEvent.From;
                    }
                }

                _destroyPool.Value.Add(i);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var i in _cleanupFilter.Value)
            {
                _eventWorld.Value.DelEntity(i);
            }
        }
    }
}