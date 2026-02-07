using Camera;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unit;
using UnityEngine;

namespace ECS
{
    internal sealed class PlayerInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsPoolInject<InputMoveComponent> _inputPool = default;
        private readonly EcsPoolInject<PlayerTag> _playerTagPool = default;
        private readonly EcsPoolInject<RigidbodyComponent> _rbPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<UnitStatsComponent> _statsPool = default;

        private readonly IPlayerCreator _creator;
        private readonly ICameraService _cameraService;
        private readonly IUnitSpawner _spawner;

        public PlayerInitSystem(IPlayerCreator creator, ICameraService cameraService, IUnitSpawner spawner)
        {
            _creator = creator;
            _cameraService = cameraService;
            _spawner = spawner;
        }

        public void Init(IEcsSystems systems)
        {
            _creator.OnNewPlayerEvent += InitializePlayer;
        }

        public void Destroy(IEcsSystems systems)
        {
            _creator.OnNewPlayerEvent -= InitializePlayer;
        }

        private void InitializePlayer(PlayerData data)
        {
            var unit = _spawner.CreateUnit(
                new RequestDto(data.Id, Layers.PlayerLayer, Layers.EnemyMask),
                Vector3.zero,
                Quaternion.identity
            );

            var entity = _world.Value.NewEntity();

            _inputPool.Value.Add(entity);
            _playerTagPool.Value.Add(entity);

            ref RigidbodyComponent rbComponent = ref _rbPool.Value.Add(entity);
            rbComponent.Value = unit.Transform.gameObject.GetComponent<Rigidbody>();

            ref var transformComponent = ref _transformPool.Value.Add(entity);
            transformComponent.Value = unit.Transform;

            ref var stats = ref _statsPool.Value.Add(entity);
            stats.Health = data.Stats.Health;
            stats.MaxHealth = data.Stats.Health;
            stats.Speed = data.Stats.Speed;

            _cameraService.SetTarget(unit.Transform);
        }
    }
}