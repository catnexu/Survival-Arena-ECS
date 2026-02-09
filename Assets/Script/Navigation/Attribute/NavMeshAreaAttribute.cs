using System;
using UnityEngine;

namespace Navigation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class NavMeshAreaAttribute : PropertyAttribute
    {
    }
}