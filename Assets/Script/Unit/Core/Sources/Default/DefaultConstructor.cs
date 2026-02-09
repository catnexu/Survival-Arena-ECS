using System;
using Infrastructure;
using UnityEngine;

namespace Unit
{
    internal sealed class DefaultConstructor : IUnitConstructor
    {
        private readonly IPoolService _poolService;

        public UnitType Type => UnitType.Default;

        public DefaultConstructor(IPoolService poolService)
        {
            _poolService = poolService;
        }

        public IUnit CreateUnit(RequestDto args, UnitConfig config, Vector3 position, Quaternion rotation)
        {
            if (config is DefaultConfig target)
            {
                return CreateUnit(target, position, rotation);
            }

            throw new Exception("Invalid config type");
        }

        private IUnit CreateUnit(DefaultConfig config, Vector3 position, Quaternion rotation)
        {
            UnitView view = _poolService.Instantiate<UnitView>(config.Prefab, position, rotation);
            PooledUnit unit = new PooledUnit(_poolService, view);
            return unit;
        }
    }
}