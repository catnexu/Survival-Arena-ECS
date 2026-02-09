using Leopotam.EcsLite;
using UnityEngine;

namespace Weapon
{
    internal sealed class EntityView : MonoBehaviour
    {
        public EcsPackedEntity Entity { get; private set; }

        public void Initialize(EcsPackedEntity entity)
        {
            Entity = entity;
        }
    }
}