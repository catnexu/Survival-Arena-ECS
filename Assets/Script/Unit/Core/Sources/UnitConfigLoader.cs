using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    internal sealed class UnitConfigLoader : IUnitConfigLoader
    {
        private const string UnitConfigDatabase = "UnitConfigDatabase";
        private readonly Dictionary<string, UnitConfig> _configMap;

        public UnitConfigLoader()
        {
            UnitConfigDatabase database = Resources.Load<UnitConfigDatabase>(UnitConfigDatabase);
            _configMap = new Dictionary<string, UnitConfig>(database.Configs.Count);
            for (int i = 0; i < database.Configs.Count; i++)
            {
                UnitConfig config = database.Configs[i];
                _configMap.Add(config.name, config);
            }
        }

        public T Load<T>(string id) where T : UnitConfig
        {
            if (_configMap.TryGetValue(id, out UnitConfig config))
            {
                if (config is T desired)
                    return desired;
                throw new Exception($"{config.name} is not of type {typeof(T).Name}");
            }

            throw new KeyNotFoundException("Unit config with id " + id + " was not found.");
        }
    }
}