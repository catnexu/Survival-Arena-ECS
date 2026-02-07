using System;
using UnityEngine;

namespace Unit
{
    public interface IUnit
    {
        event Action<IUnit> OnDestroy;
        Transform Transform { get; }
    }
}