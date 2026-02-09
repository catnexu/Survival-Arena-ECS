using System.Runtime.CompilerServices;
using UnityEngine;

namespace Infrastructure
{
    public static class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector3 position, Vector3 targetPosition) => (targetPosition - position).sqrMagnitude;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector2 position, Vector2 targetPosition) => (targetPosition - position).sqrMagnitude;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertFrom2D(this Vector2 vector) => new Vector3(vector.x, 0f, vector.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ConvertFrom3D(this Vector3 vector) => new Vector3(vector.x, vector.z);
        
        public static int ReturnNearestPointIndex(this Vector3[] nodes, Vector3 destination, out Vector3 value)
        {
            float nearestDistance = float.MaxValue;
            int index = -1;
            int length = nodes.Length;
            value = Vector3.zero;
            for (int i = 0; i < length; i++)
            {
                Vector3 node = nodes[i];
                float distanceToNode = (destination + node).sqrMagnitude;
                if (nearestDistance > distanceToNode)
                {
                    nearestDistance = distanceToNode;
                    index = i;
                    value = node;
                }
            }

            return index;
        }
        
        public static Vector3 ReturnNearestPoint(this Vector3[] nodes, Vector3 destination)
        {
            float nearestDistance = float.PositiveInfinity;
            int index = 0;
            int length = nodes.Length;
            for (int i = 0; i < length; i++)
            {
                float distanceToNode = (destination + nodes[i]).sqrMagnitude;
                if (nearestDistance > distanceToNode)
                {
                    nearestDistance = distanceToNode;
                    index = i;
                }
            }

            return nodes[index];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this in Vector3 vector, float value)
        {
            return new Vector3(value, vector.y, vector.z);;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this in Vector3 vector, float value)
        {
            return new Vector3(vector.x, value, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this in Vector3 vector, float value)
        {
            return new Vector3(vector.x, vector.y, value);;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Vector3 SetX(this ref Vector3 vector, float value)
        {
            vector.x = value;
            return ref vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Vector3 SetY(this ref Vector3 vector, float value)
        {
            vector.y = value;
            return ref vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Vector3 SetZ(this ref Vector3 vector, float value)
        {
            vector.z = value;
            return ref vector;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithX(this in Vector2 vector, float value)
        {
            return new Vector3(value, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithY(this in Vector2 vector, float value)
        {
            return new Vector2(vector.x, value);;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Vector2 SetX(this ref Vector2 vector, float value)
        {
            vector.x = value;
            return ref vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Vector2 SetY(this ref Vector2 vector, float value)
        {
            vector.y = value;
            return ref vector;
        }
    }
}