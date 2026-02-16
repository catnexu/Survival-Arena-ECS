using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    internal sealed class NavigationGridService : INavigationGridService
    {
        private const string NavigationConfig = "NavigationConfig";
        private const int RandomizeTryCount = 5;
        private readonly Dictionary<NavigationGridType, NavMeshQueryFilter> _gridFilterMap;
        private bool _alterationLock;

        public NavigationGridService()
        {
            _gridFilterMap = new Dictionary<NavigationGridType, NavMeshQueryFilter>();
            NavigationConfig configAsset = Resources.Load<NavigationConfig>(NavigationConfig);
            for (int i = 0; i < configAsset.TypeAreaIds.Count; i++)
            {
                NavigationTypeAreaId kvp = configAsset.TypeAreaIds[i];
                _gridFilterMap.Add(kvp.Type, kvp.GetFilter());
            }
        }

        public Vector3 GetRandomPositionInRadius(NavigationGridType type, Vector3 origin, float radius)
        {
            NavMeshQueryFilter filter = _gridFilterMap[type];
            for (int i = 0; i < RandomizeTryCount; i++)
            {
                if (NavMesh.SamplePosition(origin, out NavMeshHit hit, radius, filter))
                {
                    return hit.position;
                }
            }

            return GetNearestPassible(type, origin);
        }

        public Vector3 GetNearestPassible(NavigationGridType type, Vector3 origin)
        {
            return NavMesh.SamplePosition(origin, out NavMeshHit hit, float.MaxValue, _gridFilterMap[type]) ? hit.position : origin;
        }
    }
}