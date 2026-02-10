using System.Runtime.CompilerServices;

namespace Infrastructure
{
    public static class ServiceLocatorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register<T>(this IServiceLocator locator, T instance) where T : class
        {
            locator.Register(typeof(T), instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register<T1, T2>(this IServiceLocator locator, object instance)
        {
            locator.Register(typeof(T1), instance);
            locator.Register(typeof(T2), instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register<T1, T2, T3>(this IServiceLocator locator, object instance)
        {
            locator.Register(typeof(T1), instance);
            locator.Register(typeof(T2), instance);
            locator.Register(typeof(T3), instance);
        }
    }
}