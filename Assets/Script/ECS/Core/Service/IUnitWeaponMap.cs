using System.Collections.Generic;

namespace ECS
{
    public interface IUnitWeaponMap
    {
        bool TryGetWeapons(int ownerId, out IReadOnlyList<int> weapons);
        void AddWeapon(int ownerId, int entity);
        void RemoveWeapon(int ownerId, int entity);
        void Clean(int ownerId);
    }
}