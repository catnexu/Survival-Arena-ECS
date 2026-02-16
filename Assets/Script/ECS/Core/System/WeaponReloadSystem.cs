using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class WeaponReloadSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<WeaponComponent>, Exc<WeaponReadyFlag, DestroyTag>> _filter = default;
        private readonly EcsPoolInject<WeaponReadyFlag> _readyPool = default;
        private readonly ITimeManager _timeManager;

        public WeaponReloadSystem(ITimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        public void Run(IEcsSystems systems)
        {
            float deltaTime = _timeManager.DeltaTime;

            foreach (var entity in _filter.Value)
            {
                ref var weapon = ref _filter.Pools.Inc1.Get(entity);
                if (!weapon.IsCharged)
                {
                    weapon.TimeSinceLastShot += deltaTime;
                    if (weapon.TimeSinceLastShot >= weapon.ReloadTime)
                    {
                        weapon.IsCharged = true;
                        weapon.TimeSinceLastShot -= weapon.ReloadTime;
                    }
                }

                if (weapon.IsCharged && weapon.IsActive)
                {
                    _readyPool.Value.Add(entity);
                }
            }
        }
    }
}