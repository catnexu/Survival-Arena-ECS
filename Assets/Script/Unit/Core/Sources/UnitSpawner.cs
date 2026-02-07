using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    internal sealed class UnitSpawner : IUnitSpawner
    {
        private readonly Dictionary<UnitType, IUnitConstructor> _map;
        private readonly IUnitConfigLoader _loader;

        public UnitSpawner(IUnitConfigLoader loader)
        {
            _loader = loader;
            _map = new Dictionary<UnitType, IUnitConstructor>();
        }

        public void Initialize(IReadOnlyList<IUnitConstructor> constructors)
        {
            for (int i = 0; i < constructors.Count; i++)
            {
                IUnitConstructor unitConstructor = constructors[i];
                _map.Add(unitConstructor.Type, unitConstructor);
            }
        }

        public IUnit CreateUnit(RequestDto args, Vector3 position, Quaternion rotation)
        {
            UnitConfig config = _loader.Load<UnitConfig>(args.Id);
            if (_map.TryGetValue(config.Type, out IUnitConstructor constructor))
            {
                return constructor.CreateUnit(args, config, position, rotation);
            }

            throw new Exception($"There's no constructor of type {config.Type}");
        }
    }
}