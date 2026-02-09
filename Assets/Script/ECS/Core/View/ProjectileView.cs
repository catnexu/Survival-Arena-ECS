using System;
using UnityEngine;

namespace Weapon
{
    internal sealed class ProjectileView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private TriggerView _trigger;

        public event Action<ProjectileView, Collider> OnTriggerEnter;
        public event Action<ProjectileView> OnDestroy;
        public void Initialize(int mask)
        {
            LayerMask excludeLayers = -1;
            excludeLayers &= ~mask;
            _rigidbody.excludeLayers = excludeLayers;
            _trigger.OnTriggered += OnTriggered;
        }

        private void OnTriggered(Collider obj)
        {
            OnTriggerEnter?.Invoke(this, obj);
        }

        public void Destroy()
        {
            _trigger.OnTriggered -= OnTriggered;
            OnDestroy?.Invoke(this);
        }
    }
}