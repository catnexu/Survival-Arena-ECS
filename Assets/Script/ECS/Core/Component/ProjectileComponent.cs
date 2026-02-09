using Leopotam.EcsLite;
using UnityEngine;

namespace ECS
{
    internal struct ProjectileComponent
    {
        public EcsPackedEntity OwnerEntity;
        public Vector3 Direction;
        public float Speed;
        public float LifeTime;
        public float Damage;
    }
}