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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(this EcsPool<T> pool, int entity, out T component) where T : struct
        {
            component = default;
            if (pool.Has(entity))
            {
                component = ref pool.Get(entity);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T TryGetRef<T>(this EcsPool<T> pool, int entity, out bool success) where T : struct
        {
            success = pool.Has(entity);
            if (success)
                return ref pool.Get(entity);
            return ref Dummy<T>.Value;
        }

        private static class Dummy<T> where T : struct
        {
            public static T Value = default;
        }
    }
}