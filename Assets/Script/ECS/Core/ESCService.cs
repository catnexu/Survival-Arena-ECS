using Camera;
using Infrastructure;
using Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unit;

namespace ECS
{
    internal sealed class EscService : IEcsService
    {
        private readonly IServiceLocator _serviceLocator;

        public EscService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IEcsController CreateController()
        {
            var world = new EcsWorld();
            var eventWorld = new EcsWorld();
            var updateSystems = new EcsSystems(world);
            updateSystems
                .AddWorld(eventWorld, WorldNames.EVENT)
                .Add(new InputMoveUpdateSystem(_serviceLocator.Resolve<IInputService>()))
                .Add(new PlayerInitSystem(_serviceLocator.Resolve<IPlayerCreator>(), _serviceLocator.Resolve<ICameraService>(),
                    _serviceLocator.Resolve<IUnitSpawner>()))

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(WorldNames.EVENT))
#endif
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