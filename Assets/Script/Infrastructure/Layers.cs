using UnityEngine;

namespace Infrastructure
{
    public static class Layers
    {
        public static readonly int PlayerLayer = LayerMask.NameToLayer("Player");
        public static readonly int EnemyLayer = LayerMask.NameToLayer("Enemy");
        public static readonly int ObstacleLayer = LayerMask.NameToLayer("Obstacle");

        public static readonly int PlayerMask = 1 << PlayerLayer;
        public static readonly int EnemyMask = 1 << EnemyLayer;
        public static readonly int ObstacleMask = 1 << ObstacleLayer;

        public static int GetEnemyMask(int layer)
        {
            if (layer == PlayerLayer)
                return EnemyMask;
            return layer == EnemyLayer ? PlayerMask : -1;
        }
    }
}