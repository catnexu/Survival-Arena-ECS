using UnityEngine;

namespace Weapon
{
    public abstract class WeaponConfig : ScriptableObject
    {
        [SerializeField] private float _reloadTime;
        public float ReloadTime => _reloadTime;
    }
}