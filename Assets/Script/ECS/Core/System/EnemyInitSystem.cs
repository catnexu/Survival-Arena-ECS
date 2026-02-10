using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Navigation;
using UI;
using Unit;
using UnityEngine;
using UnityEngine.AI;
using Weapon;

namespace ECS
{
    internal sealed class EnemyInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;
        private readonly EcsPoolInject<UnitComponent> _unitPool = default;
        private readonly EcsPoolInject<EnemyTag> _enemyTagPool = default;
        private readonly EcsPoolInject<RigidbodyComponent> _rbPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<UnitHealthComponent> _statsPool = default;
        private readonly EcsPoolInject<LayerComponent> _layerPool = default;
        private readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        private readonly EcsPoolInject<HealthBarViewComponent> _healthBarPool = default;

        private readonly IEnemyFactory _factory;
        private readonly IUnitSpawner _spawner;
        private readonly IHealthBarFactory _healthBarFactory;

        public EnemyInitSystem(IEnemyFactory factory, IUnitSpawner spawner, IHealthBarFactory healthBarFactory)
        {
            _factory = factory;
            _spawner = spawner;
            _healthBarFactory = healthBarFactory;
        }

        public void Init(IEcsSystems systems)
        {
            _factory.OnNewUnitEvent += InitializeEnemy;
        }

        public void Destroy(IEcsSystems systems)
        {
            _factory.OnNewUnitEvent -= InitializeEnemy;
        }

        private void InitializeEnemy(EnemyData data)
        {
            IUnit unit = _spawner.CreateUnit(
                new RequestDto(data.Id),
                Vector3.zero,
                Quaternion.identity
            );
            unit.View.Initialize(Layers.EnemyLayer);
            var entity = _world.Value.NewEntity();
            var packEntity = _world.Value.PackEntity(entity);

            var entityView = unit.View.gameObject.GetOrAddComponent<EntityView>();
            entityView.Initialize(packEntity);

            _unitPool.Value.Add(entity).Value = unit;
            _enemyTagPool.Value.Add(entity);
            ref LayerComponent layerComponent = ref _layerPool.Value.Add(entity);
            layerComponent.Value = Layers.EnemyLayer;
            layerComponent.EnemyLayer = Layers.PlayerLayer;
            ref RigidbodyComponent rbComponent = ref _rbPool.Value.Add(entity);
            rbComponent.Value = unit.View.gameObject.GetComponent<Rigidbody>();

            ref var transformComponent = ref _transformPool.Value.Add(entity);
            transformComponent.Value = unit.View.transform;
            unit.View.transform.position = data.Position;

            ref var health = ref _statsPool.Value.Add(entity);
            health.Health = data.Stats.Health;
            health.MaxHealth = data.Stats.Health;

            var healthBar = _healthBarFactory.CreateHealthBar();
            healthBar.UpdateHealth(data.Stats.Health, data.Stats.Health);
            
            _healthBarPool.Value.Add(entity).Value = healthBar;

            foreach (var weapon in data.Weapons)
            {
                ref var weaponEvent = ref _eventWorld.Value.SendEvent<WeaponCreateEvent>();
                weaponEvent.Owner = packEntity;
                weaponEvent.Value = weapon;
            }

            NavMeshAgent agent = unit.View.gameObject.GetOrAddComponent<NavMeshAgent>();
            agent.SetupAgent(data.NavMeshAgentConfiguration);
            _navMeshPool.Value.Add(entity).Value = agent;
        }
    }
}