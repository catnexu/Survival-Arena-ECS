using System;
using UnityEngine;

namespace UI
{
    internal sealed class FieldViewProvider : MonoBehaviour, IFieldViewProvider
    {
        public event Action<IFieldViewProvider> OnUpdate;
        public Vector3 BottomLeft { get; private set;  }
        public Vector3 TopRight { get; private set; }

        private Camera _camera;
        private bool _initialized;

        public void Initialize(Camera viewCamera)
        {
            _camera = viewCamera;
            _initialized = true;
            UpdateSize();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            if (_initialized)
            {
                BottomLeft = _camera.ViewportToWorldPoint(Vector3.zero);
                TopRight = _camera.ViewportToWorldPoint(Vector3.one);
                OnUpdate?.Invoke(this);
            }
        }
    }
}