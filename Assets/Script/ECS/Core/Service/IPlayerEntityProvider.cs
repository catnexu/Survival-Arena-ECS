using UnityEngine;

namespace ECS
{
    internal interface IPlayerEntityProvider
    {
        bool TryGetNearestPlayer(Vector3 origin, out int entity, out Vector3 position);
    }
}