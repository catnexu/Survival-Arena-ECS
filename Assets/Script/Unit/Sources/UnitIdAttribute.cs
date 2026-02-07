using System;
using UnityEngine;

namespace Unit
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class UnitIdAttribute : PropertyAttribute
    {
    }
}