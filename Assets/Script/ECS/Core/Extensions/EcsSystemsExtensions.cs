using System.Runtime.CompilerServices;
using Infrastructure;
using Leopotam.EcsLite;

namespace ECS
{
    public static class EcsSystemsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEcsSystems OneFrameSystem<T>(this IEcsSystems systems, string worldName = null)
            where T : struct =>
            systems.Add(new OneFrameSystem<T>(systems.GetWorld(worldName)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEcsSystems DelayedAddComponentSystem<T>(this IEcsSystems systems, ITimeManager timeService, string worldName = null)
            where T : struct =>
            systems.Add(new DelayTimeAddSystem<T>(timeService, systems.GetWorld(worldName)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEcsSystems DelayedRemoveComponentSystem<T>(this IEcsSystems systems, ITimeManager timeService, string worldName = null)
            where T : struct =>
            systems.Add(new DelayTimeRemoveSystem<T>(timeService, systems.GetWorld(worldName)));
    }
}