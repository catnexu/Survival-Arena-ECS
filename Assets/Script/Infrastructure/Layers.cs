using UnityEngine;

namespace Infrastructure
{
    public static class Layers
    {
        public static readonly int PlayerLayer = LayerMask.NameToLayer("Player");
        public static readonly int EnemyLayer = LayerMask.NameToLayer("Enemy");
        
        public static readonly int PlayerMask = 1 << PlayerLayer;
        public static readonly int EnemyMask = 1 << EnemyLayer;
    }
}