using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace ECS
{
    public static class EcsWorldExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T SendEvent<T>(this EcsWorld world) where T : struct => ref world.GetPool<T>().Add(world.NewEntity());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddComponentDelayed<T>(this EcsWorld world, int entity, float seconds) where T : struct
        {
            var timeEntity = world.NewEntity();
            ref var delayComponent = ref world.GetPool<DelayTimeAddInitComponent<T>>().Add(timeEntity);
            delayComponent.Entity = world.PackEntity(entity);
            delayComponent.DelaySec = seconds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveComponentDelayed<T>(this EcsWorld world, int entity, float seconds) where T : struct
        {
            var timeEntity = world.NewEntity();
            ref var delayComponent = ref world.GetPool<DelayTimeRemoveInitComponent<T>>().Add(timeEntity);
            delayComponent.Entity = world.PackEntity(entity);
            delayComponent.DelaySec = seconds;
        }
    }
}