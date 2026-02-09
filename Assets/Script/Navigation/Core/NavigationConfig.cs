using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    [CreateAssetMenu(menuName = "Navigation/" + nameof(NavigationConfig), fileName = nameof(NavigationConfig), order = 0)]
    internal sealed class NavigationConfig : ScriptableObject
    {
        [SerializeField] private NavigationTypeAreaId[] _typeAreaIds;
        public IReadOnlyList<NavigationTypeAreaId> TypeAreaIds => _typeAreaIds;
    }
}