namespace Weapon
{
    public interface IWeaponConfigLoader
    {
        WeaponConfig Load(string id);
    }
}