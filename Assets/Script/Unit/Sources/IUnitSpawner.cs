using UnityEngine;

namespace Unit
{
    public interface IUnitSpawner
    {
        IUnit CreateUnit(RequestDto args, Vector3 position, Quaternion rotation);
    }
}