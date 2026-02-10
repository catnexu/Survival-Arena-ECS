using UnityEngine;

namespace UI
{
    internal sealed class UIStorage : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private FieldViewProvider _fieldViewProvider;
        [SerializeField] private HealthBarView _healthBarPrefab;

        public Camera UICamera => _uiCamera;
        public FieldViewProvider FieldViewProvider => _fieldViewProvider;
        public HealthBarView HealthBarPrefab => _healthBarPrefab;
    }
}