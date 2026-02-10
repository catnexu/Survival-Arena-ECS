using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapon/" + nameof(SwordConfig), fileName = nameof(SwordConfig))]
    public sealed class SwordConfig : WeaponConfig
    {
        [SerializeField] private float _range;
        [SerializeField] private float _damage;

        public float Range => _range;
        public float Damage => _damage;
    }
}