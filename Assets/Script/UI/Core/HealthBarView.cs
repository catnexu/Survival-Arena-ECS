using TMPro;
using UnityEngine;

namespace UI
{
    internal sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        public void UpdateHealth(float current, float max)
        {
            _healthText.text = Mathf.CeilToInt(current).ToString();
        }
    }
}