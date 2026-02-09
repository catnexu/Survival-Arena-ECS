using UnityEngine;

namespace UI
{
    internal sealed class FieldViewProvider : MonoBehaviour, IFieldViewProvider
    {
        public Vector3 BottomLeft => ViewToWorldPoint(_camera, new Vector2(0f, 0f));
        public Vector3 TopRight => ViewToWorldPoint(_camera, new Vector2(1f, 1f));

        private Camera _camera;

        public void Initialize(Camera viewCamera)
        {
            _camera = viewCamera;
        }

        private static Vector3 ViewToWorldPoint(Camera camera,
            Vector2 viewPoint,
            float yHeight = 0f,
            Camera.MonoOrStereoscopicEye eyeType = Camera.MonoOrStereoscopicEye.Mono)
        {
            Ray ray = camera.ViewportPointToRay(viewPoint, eyeType);
            return ProjectRayOnYPlane(ray, yHeight);
        }

        private static Vector3 ProjectRayOnYPlane(Ray ray, float yHeight)
        {
            Vector3 normalizedDirection = ray.direction.normalized;
            Vector3 origin = ray.origin;
            Vector3 projected;
            if (normalizedDirection.y != 0f)
            {
                float distanceToProjection = (1f / normalizedDirection.y) * origin.y - yHeight;
                projected = origin - normalizedDirection * distanceToProjection;
            }
            else
            {
                projected = new Vector3(origin.x, 0f, origin.z);
            }

            return projected;
        }
    }
}