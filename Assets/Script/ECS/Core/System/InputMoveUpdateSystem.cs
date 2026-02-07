using System;
using Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS
{
    internal sealed class InputMoveUpdateSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem, IPlayerInputListener
    {
        private readonly EcsFilterInject<Inc<InputMoveComponent>> _filter = default;
        private readonly IInputService _inputService;
        private IDisposable _sub;
        private Vector2 _inputValue;

        public InputMoveUpdateSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Init(IEcsSystems systems)
        {
            _sub = _inputService.AddListener(this);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter.Value)
            {
                _filter.Pools.Inc1.Get(i).Value = _inputValue;
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _sub.Dispose();
        }

        public void Move(Vector2 value)
        {
            _inputValue = value;
        }
    }
}