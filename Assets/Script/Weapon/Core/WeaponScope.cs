using Infrastructure;

namespace Weapon
{
    public static class WeaponScope
    {
        public static void Build(IServiceLocator locator)
        {
            locator.Register<IWeaponConfigLoader>(new WeaponConfigLoader());
        }
    }
}