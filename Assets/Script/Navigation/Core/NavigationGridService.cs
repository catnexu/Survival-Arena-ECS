using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    internal sealed class NavigationGridService : INavigationGridService
    {
        private const string NavigationConfig = "NavigationConfig";
        private const int RandomizeTryCount = 5;
        private readonly Dictionary<NavigationGridType, NavMeshQueryFilter> _gridFilterMap;
        private readonly IRandomizer _randomizer;
        private bool _alterationLock;
        
        public NavigationGridService(IRandomizer randomizer)
        {
            _randomizer = randomizer;
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
            return GetRandomPositionInDonut(type, origin, _randomizer.GetRandom(0f, radius), radius);
        }

        public Vector3 GetRandomPositionInDonut(NavigationGridType type, Vector3 origin, float minRadius, float maxRadius)
        {
            NavMeshQueryFilter filter = _gridFilterMap[type];
            float searchRadius = (maxRadius - minRadius) * 0.5f;
            for (int i = 0; i < RandomizeTryCount; i++)
            {
                Vector3 direction = _randomizer.OnUnitCircle().ConvertFrom2D() * (maxRadius - searchRadius);
                if (NavMesh.SamplePosition(origin + direction, out NavMeshHit hit, searchRadius, filter))
                {
                    return hit.position;
                }
            }

            return GetNearestPassible(type, origin);
        }

        public Vector3 GetRandomPositionInSegment(NavigationGridType type, Vector3 origin, Vector3 direction, float halfAngle, float radius)
        {
            return GetRandomPositionInDonutSegment(type, origin, direction, halfAngle, 0f, radius);
        }

        public Vector3 GetRandomPositionInDonutSegment(NavigationGridType type, Vector3 origin, Vector3 direction, float halfAngle,
            float minRadius, float maxRadius)
        {
            float searchRadius = (maxRadius - minRadius) * 0.5f;
            for (int i = 0; i < RandomizeTryCount; i++)
            {
                float randomRadius = _randomizer.GetRandom(minRadius, maxRadius);
                float randomAngle = _randomizer.GetRandom(-halfAngle, halfAngle);
                Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * direction.normalized;
                Vector3 randomPosition = origin + randomDirection * randomRadius;
                if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, searchRadius, _gridFilterMap[type]))
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