using Camera;
using Infrastructure;
using Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unit;
using Weapon;

namespace ECS
{
    internal sealed class EscService : IEcsService
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly UnitWeaponMap _weaponMap;

        public EscService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _weaponMap = new UnitWeaponMap();
        }

        public IEcsController CreateController()
        {
            _weaponMap.Clear();
            var world = new EcsWorld();
            var eventWorld = new EcsWorld();
            var updateSystems = new EcsSystems(world);

            updateSystems
                .AddWorld(eventWorld, WorldNames.EVENT)
                .Add(new InputMoveUpdateSystem(_serviceLocator.Resolve<IInputService>()))
                .Add(new PlayerInitSystem(_serviceLocator.Resolve<IPlayerCreator>(), _serviceLocator.Resolve<ICameraService>(),
                    _serviceLocator.Resolve<IUnitSpawner>()))
                .Add(new WeaponInitSystem(_serviceLocator.Resolve<IWeaponConfigLoader>(), _weaponMap))
                .Add(new WeaponReloadSystem(_serviceLocator.Resolve<ITimeManager>()))
                .Add(new PlayerSetTargetSystem(_serviceLocator.Resolve<IRandomizer>(), _weaponMap))
                .Add(new GunShootingSystem(_serviceLocator.Resolve<IPoolService>()))
                .Add(new ProjectileMovementSystem(_serviceLocator.Resolve<ITimeManager>()))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(WorldNames.EVENT))
#endif
                .Add(new UnitCleanupSystem(_weaponMap))
                .Add(new WeaponCleanupSystem(_weaponMap))
                .Add(new ProjectileCleanupSystem(_serviceLocator.Resolve<IPoolService>()))
                .OneFrameSystem<WeaponCreateEvent>(eventWorld)
                .OneFrameSystem<DamageEvent>(eventWorld)
                .OneFrameSystem<WeaponTargetComponent>()
                .Add(new EntityCleanupSystem(world))
                .Add(new EntityCleanupSystem(eventWorld))
                .Inject();

            var fixedUpdateSystems = new EcsSystems(world);
            fixedUpdateSystems
                .AddWorld(eventWorld, WorldNames.EVENT)
                .Add(new PlayerMovementSystem())
                .Inject();
            return new EcsController(_serviceLocator.Resolve<ITickController>(), updateSystems, fixedUpdateSystems, new[] {world, eventWorld});
        }
    }
}