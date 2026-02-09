using System;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    [Serializable]
    internal struct NavigationTypeAreaId
    {
        [SerializeField] private NavigationGridType _type;
        [SerializeField, NavMeshArea] private int[] _areaIds;
        [SerializeField, NavMeshAgent] private int _agentTypeId;
        public NavigationGridType Type => _type;

        public NavMeshQueryFilter GetFilter()
        {
            int mask = 0;
            for (int i = 0; i < _areaIds.Length; i++)
            {
                mask |= (1 << _areaIds[i]);
            }

            return new NavMeshQueryFilter {areaMask = mask, agentTypeID = _agentTypeId};
        }
    }
}