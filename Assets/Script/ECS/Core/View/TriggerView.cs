using System;
using UnityEngine;

namespace Weapon
{
    internal sealed class TriggerView : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        public event Action<Collider> OnTriggered;

        private void OnTriggerEnter(Collider other) => OnTriggered?.Invoke(other);
    }
}