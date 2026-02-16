using Infrastructure;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Weapon;

namespace ECS
{
    internal sealed class WeaponInitSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<WeaponCreateEvent>> _eventFilter = WorldNames.EVENT;
        private readonly EcsPoolInject<LayerComponent> _layerPool = default;
        private readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        private readonly EcsPoolInject<GunComponent> _gunPool = default;
        private readonly EcsPoolInject<SwordComponent> _swordPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<WeaponMuzzleComponent> _muzzlePool = default;
        private readonly IWeaponConfigLoader _loader;
        private readonly IUnitWeaponMap _weaponMap;
        private readonly IPoolService _poolService;
        private readonly GameObject _muzzlePrefab;

        public WeaponInitSystem(IWeaponConfigLoader loader, IUnitWeaponMap weaponMap, IPoolService poolService)
        {
            _loader = loader;
            _weaponMap = weaponMap;
            _poolService = poolService;
            _muzzlePrefab = new GameObject("Muzzle");
            _muzzlePrefab.SetActive(false);
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
            var muzzle = _poolService.Instantiate<Transform>(_muzzlePrefab, Vector3.zero, Quaternion.identity);
            muzzle.SetParent(ownerTransform.Value);
            muzzle.localPosition = Vector3.up;
            _muzzlePool.Value.Add(weaponEntity).Value = muzzle;

            switch (config)
            {
                case GunConfig gunConfig:
                {
                    ref GunComponent gun = ref _gunPool.Value.Add(weaponEntity);
                    gun.ProjectileDamage = gunConfig.ProjectileDamage;
                    gun.ProjectilePrefab = gunConfig.ProjectilePrefab;
                    gun.ProjectileSpeed = gunConfig.ProjectileSpeed;
                    gun.ProjectileLifetime = gunConfig.ProjectileLifetime;
                    break;
                }
                case SwordConfig swordConfig:
                {
                    ref SwordComponent sword = ref _swordPool.Value.Add(weaponEntity);
                    sword.Damage = swordConfig.Damage;
                    sword.Range = swordConfig.Range;
                    Color color = Random.ColorHSV();
                    color.a = 0.3f;
                    sword.DrawColor = color;
                    break;
                }
            }

            _weaponMap.AddWeapon(ownerUnpacked, weaponEntity);
        }

        public void Destroy(IEcsSystems systems)
        {
            _poolService.Clear(_muzzlePrefab);
            Object.Destroy(_muzzlePrefab);
        }
    }
}