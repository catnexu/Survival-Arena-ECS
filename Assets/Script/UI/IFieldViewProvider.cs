using UnityEngine;

namespace UI
{
    public interface IFieldViewProvider
    {
        Vector3 BottomLeft { get; }
        Vector3 TopRight { get; }
    }
}