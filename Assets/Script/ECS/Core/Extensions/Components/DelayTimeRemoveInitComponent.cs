using Leopotam.EcsLite;

namespace ECS
{
    internal struct DelayTimeRemoveInitComponent<T> where T : struct
    {
        public float DelaySec;
        public EcsPackedEntity Entity;
    }
}