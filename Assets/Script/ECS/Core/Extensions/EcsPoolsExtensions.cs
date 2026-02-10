using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace ECS
{
    public static class EcsPoolsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Replace<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
            {
                pool.Del(entity);
            }

            return ref pool.Add(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
            {
                return ref pool.Add(entity);
            }

            return ref pool.Get(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryDel<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
            {
                pool.Del(entity);
            }
        }
    }
}