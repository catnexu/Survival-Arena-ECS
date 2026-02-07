using UnityEngine;

namespace Unit
{
    public interface IUnitConstructor
    {
        UnitType Type { get; }
        IUnit CreateUnit(RequestDto args, UnitConfig config, Vector3 position, Quaternion rotation);
    }
}