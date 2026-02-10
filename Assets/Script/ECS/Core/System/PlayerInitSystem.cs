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
        private readonly EcsPoolInject<UnitHealthComponent> _statsPool = default;
        private readonly EcsPoolInject<LayerComponent> _layerPool = default;
        private readonly EcsPoolInject<UnitSpeedComponent> _speedPool = default;
        private readonly EcsPoolInject<UnitCoinStorageComponent> _coinStoragePool = default;

        private readonly IPlayerFactory _factory;
        private readonly ICameraService _cameraService;
        private readonly IUnitSpawner _spawner;

        public PlayerInitSystem(IPlayerFactory factory, ICameraService cameraService, IUnitSpawner spawner)
        {
            _factory = factory;
            _cameraService = cameraService;
            _spawner = spawner;
        }

        public void Init(IEcsSystems systems)
        {
            _factory.OnNewUnitEvent += InitializePlayer;
        }

        public void Destroy(IEcsSystems systems)
        {
            _factory.OnNewUnitEvent -= InitializePlayer;
        }

        private void InitializePlayer(PlayerData data)
        {
            IUnit unit = _spawner.CreateUnit(
                new RequestDto(data.Id),
                Vector3.zero,
                Quaternion.identity
            );
            unit.View.Initialize(Layers.PlayerLayer);
            var entity = _world.Value.NewEntity();
            var packEntity = _world.Value.PackEntity(entity);
            
            var entityView = unit.View.gameObject.GetOrAddComponent<EntityView>();
            entityView.Initialize(packEntity);

            _unitPool.Value.Add(entity).Value = unit;
            _inputPool.Value.Add(entity);
            _playerTagPool.Value.Add(entity);
            ref LayerComponent layerComponent = ref _layerPool.Value.Add(entity);
            layerComponent.Value = Layers.PlayerLayer;
            layerComponent.EnemyLayer = Layers.EnemyLayer;
            ref RigidbodyComponent rbComponent = ref _rbPool.Value.Add(entity);
            rbComponent.Value = unit.View.gameObject.GetComponent<Rigidbody>();
            
            ref var transformComponent = ref _transformPool.Value.Add(entity);
            transformComponent.Value = unit.View.transform;
            
            _coinStoragePool.Value.Add(entity).Value = 0;

            ref var stats = ref _statsPool.Value.Add(entity);
            stats.Health = data.Stats.Health;
            stats.MaxHealth = data.Stats.Health;
            _speedPool.Value.Add(entity).Value = data.Speed;

            _cameraService.SetTarget(unit.View.transform);

            foreach (var weapon in data.Weapons)
            {
                ref var weaponEvent = ref _eventWorld.Value.SendEvent<WeaponCreateEvent>();
                weaponEvent.Owner = packEntity;
                weaponEvent.Value = weapon;
            }
        }
    }
}