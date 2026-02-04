using UnityEngine;

namespace UI
{
    internal sealed class UIStorage : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private FieldViewProvider _fieldViewProvider;

        public Camera UICamera => _uiCamera;
        public FieldViewProvider FieldViewProvider => _fieldViewProvider;
    }
}