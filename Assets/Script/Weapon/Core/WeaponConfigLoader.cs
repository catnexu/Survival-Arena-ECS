using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    internal sealed class WeaponConfigLoader : IWeaponConfigLoader
    {
        private const string WeaponConfigDatabase = "WeaponConfigDatabase";
        private readonly Dictionary<string, WeaponConfig> _configMap;

        public WeaponConfigLoader()
        {
            WeaponConfigDatabase database = Resources.Load<WeaponConfigDatabase>(WeaponConfigDatabase);
            _configMap = new Dictionary<string, WeaponConfig>(database.Configs.Count);
            for (int i = 0; i < database.Configs.Count; i++)
            {
                WeaponConfig config = database.Configs[i];
                _configMap.Add(config.name, config);
            }
        }

        public WeaponConfig Load(string id)
        {
            if (_configMap.TryGetValue(id, out WeaponConfig config))
            {
                return config;
            }

            throw new KeyNotFoundException("Weapon config with id " + id + " was not found.");
        }
    }
}