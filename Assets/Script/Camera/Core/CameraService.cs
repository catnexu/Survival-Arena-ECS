using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace Camera
{
    internal sealed class CameraService : ICameraService
    {
        private readonly CinemachineCamera _virtualCamera;

        public UnityEngine.Camera MainCamera { get; }

        public CameraService(UnityEngine.Camera camera)
        {
            MainCamera = camera;
            _virtualCamera = camera.GetComponent<CinemachineCamera>();
            Assert.IsNotNull(_virtualCamera, $"Camera must contain a {nameof(CinemachineCamera)}");
        }

        public void SetTarget(Transform target)
        {
            _virtualCamera.Follow = target;
        }
    }
}