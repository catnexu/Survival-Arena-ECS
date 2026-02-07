using Infrastructure;

namespace Camera
{
    public static class CameraScope
    {
        public static void Build(IServiceLocator locator, UnityEngine.Camera camera)
        {
            locator.Register<ICameraService>(new CameraService(camera));
        }
    }
}