using UnityEngine;

namespace ECS
{
    internal struct GunComponent
    {
        public GameObject ProjectilePrefab;
        public float ProjectileDamage;
        public float ProjectileSpeed;
        public float ProjectileLifetime;
    }
}