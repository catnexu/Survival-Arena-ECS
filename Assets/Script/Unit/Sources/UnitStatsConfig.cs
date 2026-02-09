using System;
using UnityEngine;

namespace Unit
{
    [Serializable]
    public struct UnitStatsConfig
    {
        [SerializeField, Min(0)] private float _health;
        public float Health => _health;
    }
}