using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapon/" + nameof(GunConfig), fileName = nameof(GunConfig))]
    public sealed class GunConfig : WeaponConfig
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileDamage;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileLifetime;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public float ProjectileDamage => _projectileDamage;
        public float ProjectileSpeed => _projectileSpeed;
        public float ProjectileLifetime => _projectileLifetime;
    }
}