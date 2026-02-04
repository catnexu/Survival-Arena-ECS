using UnityEngine;

namespace Infrastructure
{
    public interface IPoolService
    {
        T Instantiate<T>(GameObject prefab, Vector3 position, Quaternion rotation, float lifeTime = -1f) where T : Component;
        void Destroy(GameObject gameObject);
    }
}