using UnityEngine;

namespace Camera
{
    public interface ICameraService
    {
        UnityEngine.Camera MainCamera { get; }
        void SetTarget(Transform target);
    }
}