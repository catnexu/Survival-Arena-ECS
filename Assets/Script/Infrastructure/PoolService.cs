using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Infrastructure
{
    internal sealed class PoolService : IPoolService, IUpdate, IDisposable
    {
        private class LifeTimeHandler : IEquatable<LifeTimeHandler>
        {
            private readonly PoolService _poolService;
            private readonly GameObject _instance;
            private float _lifeTime;

            public LifeTimeHandler(PoolService poolService, GameObject instance, float lifeTime)
            {
                _poolService = poolService;
                _instance = instance;
                _lifeTime = lifeTime;
            }

            public void Update(float deltaTime)
            {
                _lifeTime -= deltaTime;
                if (_lifeTime <= 0)
                {
                    _lifeTime = float.PositiveInfinity;
                    _poolService._lifeTimeHandlers.Remove(this);
                    _poolService.Destroy(_instance);
                }
            }

            public bool Equals(LifeTimeHandler other)
            {
                if (other is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Equals(_instance, other._instance);
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((LifeTimeHandler) obj);
            }

            public override int GetHashCode()
            {
                return (_instance != null ? _instance.GetHashCode() : 0);
            }

            public static bool operator ==(LifeTimeHandler left, LifeTimeHandler right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(LifeTimeHandler left, LifeTimeHandler right)
            {
                return !Equals(left, right);
            }
        }

        private readonly Dictionary<string, ObjectPool<GameObject>> _poolMap;
        private readonly List<LifeTimeHandler> _lifeTimeHandlers;
        private readonly IDisposable _updateSub;

        public PoolService(ITickController tickController)
        {
            _poolMap = new Dictionary<string, ObjectPool<GameObject>>();
            _lifeTimeHandlers = new List<LifeTimeHandler>();
            _updateSub = tickController.AddController(this);
        }

        public void Dispose()
        {
            _updateSub.Dispose();
        }

        public T Instantiate<T>(GameObject prefab, Vector3 position, Quaternion rotation, float lifeTime = -1f) where T : Component
        {
            if (!prefab)
            {
                return null;
            }

            if (!_poolMap.TryGetValue(prefab.name, out ObjectPool<GameObject> viewPool))
            {
                viewPool = new ObjectPool<GameObject>(() => CreateInstance(prefab), EnableGameObject, DisableGameObject, DestroyGameObject,
                    false);
                _poolMap[prefab.name] = viewPool;
            }

            GameObject view = viewPool.Get();
            if (!view.TryGetComponent(out T component))
            {
                viewPool.Release(view);
                throw new Exception($"{prefab.name} does not have a component of type {typeof(T).Name}");
            }
            view.transform.SetPositionAndRotation(position, rotation);
            if (lifeTime > 0f)
            {
                _lifeTimeHandlers.Add(new LifeTimeHandler(this, view, lifeTime));
            }

            return component;
        }

        public void Destroy(GameObject gameObject)
        {
            if (!gameObject)
            {
                return;
            }

            if (_poolMap.TryGetValue(gameObject.name, out ObjectPool<GameObject> pool))
            {
                pool.Release(gameObject);
            }
            else
            {
                Object.Destroy(gameObject);
            }
        }

        private static GameObject CreateInstance(GameObject prefab)
        {
            GameObject instance = Object.Instantiate(prefab);
            instance.name = prefab.name;
            return instance;
        }

        private static void DisableGameObject(GameObject gameObject) => gameObject.SetActive(false);
        private static void EnableGameObject(GameObject gameObject) => gameObject.SetActive(true);
        private static void DestroyGameObject(GameObject gameObject) => Object.Destroy(gameObject);

        void IUpdate.UpdateController(float deltaTime)
        {
            for (int i = 0; i < _lifeTimeHandlers.Count; i++)
            {
                _lifeTimeHandlers[i].Update(deltaTime);
            }
        }
    }
}