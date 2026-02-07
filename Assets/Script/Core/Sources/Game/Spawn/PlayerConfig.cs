using System;
using Unit;
using UnityEngine;

namespace Core
{
    [Serializable]
    internal struct PlayerConfig
    {
        [SerializeField, UnitId] private string _id;
        [SerializeField] private UnitStatsConfig _stats;

        public string Id => _id;
        public UnitStatsConfig Stats => _stats;
    }
}