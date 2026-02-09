using System;
using UnityEngine;

namespace Unit
{
    public abstract class Unit : IUnit
    {
        public event Action<IUnit> OnDestroy;
        private bool _destroyed;
        public UnitView View { get; }

        protected Unit(UnitView view)
        {
            View = view;
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