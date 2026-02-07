using Infrastructure;
using Weapon;

namespace Unit
{
    public static class UnitScope
    {
        public static void Build(IServiceLocator locator)
        {
            UnitConfigLoader loader = new UnitConfigLoader();
            locator.Register<IUnitConfigLoader>(loader);
            UnitSpawner spawner = new UnitSpawner(loader);
            locator.Register<IUnitSpawner>(spawner);
            spawner.Initialize(ScopeConstructors(locator));
        }

        private static IUnitConstructor[] ScopeConstructors(IServiceLocator locator)
        {
            return new IUnitConstructor[] {new DefaultConstructor(locator.Resolve<IPoolService>())};
        }
    }
}