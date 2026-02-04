using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Infrastructure
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAdd<T>(this IList<T> list, object item)
        {
            if (item is T target)
            {
                list.Add(target);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemove<T>(this IList<T> list, object item)
        {
            if (item is T target)
            {
                return list.Remove(target);
            }

            return false;
        }
    }
}