using Infrastructure;

namespace Navigation
{
    public static class NavigationScope
    {
        public static void Build(IServiceLocator locator)
        {
            locator.Register<INavigationGridService>(new NavigationGridService());
        }
    }
}