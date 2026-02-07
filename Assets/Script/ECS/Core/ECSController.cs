using System;
using System.Collections.Generic;
using Infrastructure;
using Leopotam.EcsLite;

namespace ECS
{
    internal sealed class EcsController : IEcsController, IDisposable, IUpdate, IFixedUpdate
    {
        private readonly ITickController _tickController;
        private readonly IEcsSystems _updateSystems;
        private readonly IEcsSystems _fixedUpdateSystems;
        private readonly IReadOnlyList<EcsWorld> _worlds;
        private IDisposable _sub;
        private bool _started;

        public EcsController(ITickController tickController, IEcsSystems updateSystems, IEcsSystems fixedUpdateSystems,
            IReadOnlyList<EcsWorld> worlds)
        {
            _tickController = tickController;
            _updateSystems = updateSystems;
            _fixedUpdateSystems = fixedUpdateSystems;
            _worlds = worlds;
        }

        public void Start()
        {
            if (!_started)
            {
                _started = true;
                _updateSystems.Init();
                _fixedUpdateSystems.Init();
                _sub = _tickController.AddController(this);
            }
        }

        public void Dispose()
        {
            _sub?.Dispose();
            _updateSystems.Destroy();
            _fixedUpdateSystems.Destroy();
            for (int i = 0; i < _worlds.Count; i++)
            {
                _worlds[i].Destroy();
            }
        }

        void IUpdate.UpdateController(float deltaTime) => _updateSystems.Run();

        void IFixedUpdate.UpdateController(float deltaTime) => _fixedUpdateSystems.Run();
    }
}