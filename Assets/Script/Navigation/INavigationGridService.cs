using UnityEngine;

namespace Navigation
{
    public interface INavigationGridService
    {
        Vector3 GetRandomPositionInRadius(NavigationGridType type, Vector3 origin, float radius);
        Vector3 GetNearestPassible(NavigationGridType type, Vector3 origin);
    }
}