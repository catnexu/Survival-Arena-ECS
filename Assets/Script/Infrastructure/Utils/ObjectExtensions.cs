using UnityEngine;

namespace Infrastructure
{
    public static class ObjectExtensions
    {
        public static bool IsLayerOfMask(this GameObject gameObject, int layerMask) => layerMask == (layerMask | 1 << gameObject.layer);

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
#if UNITY_2019_2_OR_NEWER
            if (!gameObject.TryGetComponent(out T component))
            {
                component = gameObject.AddComponent<T>();
            }
#else
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
#endif
            return component;
        }
    }
}