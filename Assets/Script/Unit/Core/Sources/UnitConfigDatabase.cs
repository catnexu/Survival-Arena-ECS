using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(menuName = "Database/" + nameof(UnitConfigDatabase), fileName = nameof(UnitConfigDatabase), order = 0)]
    internal sealed class UnitConfigDatabase : ScriptableObject
    {
        [SerializeField] private UnitConfig[] _configs;

        public IReadOnlyList<UnitConfig> Configs => _configs;
    }
}