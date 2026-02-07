using System;
using UnityEngine;

namespace Unit
{
    [Serializable]
    public struct UnitStatsConfig
    {
        [SerializeField, Min(0)] private float _health;
        [SerializeField, Min(0)] private float _speed;

        public float Health => _health;
        public float Speed => _speed;
    }
}