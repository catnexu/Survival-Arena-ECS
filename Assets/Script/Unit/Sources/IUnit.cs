using UnityEngine;

namespace Unit
{
    public interface IUnit
    {
        Transform Transform { get; }
        void Destroy();
    }
}