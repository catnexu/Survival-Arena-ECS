using System;
using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Weapon;

namespace ECS
{
    internal sealed class GunShootingSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;

        private readonly
            EcsFilterInject<Inc<WeaponComponent, WeaponReadyFlag, WeaponMuzzleComponent, WeaponTargetComponent, GunComponent>, Exc<DestroyTag>>
            _filter = default;

        private readonly EcsPoolInject<ProjectileComponent> _projectilePool = default;
        private readonly EcsPoolInject<ProjectileViewComponent> _projectileViewPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = default;

        private readonly IPoolService _poolService;

        public GunShootingSystem(IPoolService poolService)
        {
            _poolService = poolService;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref WeaponComponent weapon = ref _filter.Pools.Inc1.Get(entity);
                ref GunComponent gun = ref _filter.Pools.Inc5.Get(entity);
                ref WeaponMuzzleComponent muzzle = ref _filter.Pools.Inc3.Get(entity);
                ref WeaponTargetComponent target = ref _filter.Pools.Inc4.Get(entity);

                Vector3 startPos = muzzle.Value.position;
                Vector3 endPos = target.TargetPosition.WithY(muzzle.Value.position.y);
                Vector3 direction = (endPos - startPos).normalized;

                int projectileEntity = _world.Value.NewEntity();

                ref var projectile = ref _projectilePool.Value.Add(projectileEntity);
                projectile.OwnerEntity = weapon.Owner;
                projectile.Direction = direction;
                projectile.Speed = gun.ProjectileSpeed;
                projectile.LifeTime = gun.ProjectileLifetime;
                projectile.Damage = gun.ProjectileDamage;

                ProjectileView view = _poolService.Instantiate<ProjectileView>(gun.ProjectilePrefab, startPos, Quaternion.identity);
                view.Initialize(1 << weapon.TargetLayer | Layers.ObstacleMask);
                var entityView = view.gameObject.GetOrAddComponent<EntityView>();
                entityView.Initialize(_world.Value.PackEntity(projectileEntity));
                view.OnTriggerEnter += CheckCollisionEvent;
                view.OnDestroy += Unsubscribe;

                ref var viewComponent = ref _projectileViewPool.Value.Add(projectileEntity);
                viewComponent.Transform = view.transform;
                viewComponent.View = view;

                ref var transformComponent = ref _transformPool.Value.Add(projectileEntity);
                transformComponent.Value = view.transform;

                weapon.IsCharged = false;
                weapon.TimeSinceLastShot = 0f;

                _filter.Pools.Inc2.Del(entity);
            }
        }

        private void CheckCollisionEvent(ProjectileView view, Collider collider)
        {
            if (view.TryGetComponent(out EntityView entityView) && entityView.Entity.Unpack(_world.Value, out var entity))
            {
                if (collider.transform.parent.TryGetComponent(out EntityView otherView))
                {
                    ref ProjectileComponent projectileComponent = ref _projectilePool.Value.Get(entity);
                    ref DamageEvent damageEvent = ref _eventWorld.Value.SendEvent<DamageEvent>();
                    damageEvent.Damage = projectileComponent.Damage;
                    damageEvent.From = projectileComponent.OwnerEntity;
                    damageEvent.To = otherView.Entity;
                }

                Unsubscribe(view);
                _destroyPool.Value.Add(entity);
            }
            else
            {
                throw new Exception($"Entity of the projectile {view} is not exists");
            }
        }

        private void Unsubscribe(ProjectileView view)
        {
            view.OnTriggerEnter -= CheckCollisionEvent;
            view.OnDestroy -= Unsubscribe;
        }
    }
}