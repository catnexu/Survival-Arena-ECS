using UnityEngine;

namespace UI
{
    internal sealed class UIStorage : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private FieldViewProvider _fieldViewProvider;
        [SerializeField] private PlayerHudView _playerHud;
        [SerializeField] private HealthBarView _healthBarPrefab;
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private RestartWindow _restartWindow;

        public Camera UICamera => _uiCamera;
        public FieldViewProvider FieldViewProvider => _fieldViewProvider;
        public PlayerHudView PlayerHud => _playerHud;
        public HealthBarView HealthBarPrefab => _healthBarPrefab;
        public StartWindow StartWindow => _startWindow;
        public RestartWindow RestartWindow => _restartWindow;
    }
}