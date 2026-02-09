using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Weapon;

namespace ECS
{
    internal sealed class WeaponInitSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<WeaponCreateEvent>> _eventFilter = WorldNames.EVENT;
        private readonly EcsPoolInject<LayerComponent> _layerPool = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly EcsPoolInject<GunComponent> _gunPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<WeaponMuzzleComponent> _muzzlePool = default;
        private readonly IWeaponConfigLoader _loader;
        private readonly IUnitWeaponMap _weaponMap;

        public WeaponInitSystem(IWeaponConfigLoader loader, IUnitWeaponMap weaponMap)
        {
            _loader = loader;
            _weaponMap = weaponMap;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref WeaponCreateEvent value = ref _eventFilter.Pools.Inc1.Get(i);
                if (value.Owner.Unpack(_world.Value, out var entity))
                {
                    CreateWeapon(value.Owner, entity, value.Value, _layerPool.Value.Get(entity).EnemyLayer);
                }
            }
        }

        private void CreateWeapon(EcsPackedEntity ownerPacked, int ownerUnpacked, string id, int targetLayer)
        {
            WeaponConfig config = _loader.Load(id);

            var weaponEntity = _world.Value.NewEntity();
            ref WeaponComponent weapon = ref _weaponPool.Value.Add(weaponEntity);
            weapon.Owner = ownerPacked;
            weapon.ReloadTime = config.ReloadTime;
            weapon.TimeSinceLastShot = 0f;
            weapon.IsCharged = true;
            weapon.IsActive = true;
            weapon.TargetLayer = targetLayer;

            var ownerTransform = _transformPool.Value.Get(ownerUnpacked);
            _muzzlePool.Value.Add(weaponEntity).Value = ownerTransform.Value;

            if (config is GunConfig gunConfig)
            {
                ref GunComponent gun = ref _gunPool.Value.Add(weaponEntity);
                gun.ProjectileDamage = gunConfig.ProjectileDamage;
                gun.ProjectilePrefab = gunConfig.ProjectilePrefab;
                gun.ProjectileSpeed = gunConfig.ProjectileSpeed;
                gun.ProjectileLifetime = gunConfig.ProjectileLifetime;
            }

            _weaponMap.AddWeapon(ownerUnpacked, weaponEntity);
        }
    }
}