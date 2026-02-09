using Camera;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unit;
using UnityEngine;
using Weapon;

namespace ECS
{
    internal sealed class PlayerInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;
        private readonly EcsPoolInject<UnitComponent> _unitPool = default;
        private readonly EcsPoolInject<InputMoveComponent> _inputPool = default;
        private readonly EcsPoolInject<PlayerTag> _playerTagPool = default;
        private readonly EcsPoolInject<RigidbodyComponent> _rbPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<UnitStatsComponent> _statsPool = default;
        private readonly EcsPoolInject<LayerComponent> _layerPool = default;

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
            IUnit unit = _spawner.CreateUnit(
                new RequestDto(data.Id),
                Vector3.zero,
                Quaternion.identity
            );
            unit.View.Initialize(Layers.PlayerLayer);
            var playerEntity = _world.Value.NewEntity();
            var packedPlayer = _world.Value.PackEntity(playerEntity);
            
            var entityView = unit.View.gameObject.GetOrAddComponent<EntityView>();
            entityView.Initialize(packedPlayer);

            _unitPool.Value.Add(playerEntity).Value = unit;
            _inputPool.Value.Add(playerEntity);
            _playerTagPool.Value.Add(playerEntity);
            ref LayerComponent layerComponent = ref _layerPool.Value.Add(playerEntity);
            layerComponent.Value = Layers.PlayerLayer;
            layerComponent.EnemyLayer = Layers.EnemyLayer;
            ref RigidbodyComponent rbComponent = ref _rbPool.Value.Add(playerEntity);
            rbComponent.Value = unit.View.gameObject.GetComponent<Rigidbody>();
            

            ref var transformComponent = ref _transformPool.Value.Add(playerEntity);
            transformComponent.Value = unit.View.transform;

            ref var stats = ref _statsPool.Value.Add(playerEntity);
            stats.Health = data.Stats.Health;
            stats.MaxHealth = data.Stats.Health;
            stats.Speed = data.Stats.Speed;

            _cameraService.SetTarget(unit.View.transform);


            foreach (var weapon in data.Weapons)
            {
                ref var weaponEvent = ref _eventWorld.Value.SendEvent<WeaponCreateEvent>();
                weaponEvent.Owner = packedPlayer;
                weaponEvent.Value = weapon;
            }
        }
    }
}