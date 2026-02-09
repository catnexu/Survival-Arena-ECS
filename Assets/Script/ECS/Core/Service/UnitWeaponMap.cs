using System.Collections.Generic;
using UnityEngine.Pool;

namespace ECS
{
    internal sealed class UnitWeaponMap : IUnitWeaponMap
    {
        private readonly Dictionary<int, List<int>> _map = new();

        public bool TryGetWeapons(int ownerId, out IReadOnlyList<int> weapons)
        {
            if (_map.TryGetValue(ownerId, out List<int> list))
            {
                weapons = list;
                return true;
            }

            weapons = null;
            return false;
        }

        public void Clean(int ownerId)
        {
            if (_map.TryGetValue(ownerId, out List<int> list))
            {
                ListPool<int>.Release(list);
                _map.Remove(ownerId);
            }
        }

        public void AddWeapon(int ownerId, int entity)
        {
            if (!_map.TryGetValue(ownerId, out List<int> list))
            {
                list = ListPool<int>.Get();
                _map[ownerId] = list;
            }

            list.Add(entity);
        }

        public void RemoveWeapon(int ownerId, int entity)
        {
            if (_map.TryGetValue(ownerId, out List<int> list))
            {
                list.Remove(entity);
                if (list.Count == 0)
                {
                    ListPool<int>.Release(list);
                    _map.Remove(ownerId);
                }
            }
        }

        public void Clear()
        {
            foreach ((_, List<int> value) in _map)
            {
                ListPool<int>.Release(value);
            }

            _map.Clear();
        }
    }
}