using System;
using UnityEngine;

namespace Unit
{
    public abstract class Unit : IUnit
    {
        public event Action<IUnit> OnDestroy;
        private bool _destroyed;
        public string Name { get; }
        public Transform Transform { get; }

        protected Unit(string name, Transform transform)
        {
            Transform = transform;
            Name = name;
        }

        public void Destroy()
        {
            if (_destroyed)
                return;
            _destroyed = true;
            OnDestroy?.Invoke(this);
            OnDestroyProtected();
        }

        protected abstract void OnDestroyProtected();
    }
}