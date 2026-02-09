using System;
using UnityEngine;

namespace Weapon
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class WeaponIdAttribute : PropertyAttribute
    {
    }
}