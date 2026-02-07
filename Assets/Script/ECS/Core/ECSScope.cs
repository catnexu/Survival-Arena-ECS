using Infrastructure;

namespace ECS
{
    public static class EcsScope
    {
        public static void Build(IServiceLocator locator)
        {
            locator.Register<IEcsService>(new EscService(locator));
        }
    }
}