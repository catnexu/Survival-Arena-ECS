using System;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using Weapon;

namespace Core
{
    [Serializable]
    internal struct PlayerConfig
    {
        [SerializeField, UnitId] private string _id;
        [SerializeField] private UnitStatsConfig _stats;
        [SerializeField, WeaponId] private string[] _weapons;
        [SerializeField] private float _speed;

        public string Id => _id;
        public UnitStatsConfig Stats => _stats;
        public IReadOnlyList<string> Weapons => _weapons;
        public float Speed => _speed;
    }
}