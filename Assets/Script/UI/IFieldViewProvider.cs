using System;
using UnityEngine;

namespace UI
{
    public interface IFieldViewProvider
    {
        event Action<IFieldViewProvider> OnUpdate;
        Vector3 BottomLeft { get; }
        Vector3 TopRight { get; }
    }
}