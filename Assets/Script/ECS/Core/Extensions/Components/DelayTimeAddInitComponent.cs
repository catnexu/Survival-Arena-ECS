using Leopotam.EcsLite;

namespace ECS
{
    internal struct DelayTimeAddInitComponent<T> where T : struct
    {
        public float DelaySec;
        public EcsPackedEntity Entity;
    }
}