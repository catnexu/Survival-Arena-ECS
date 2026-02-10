using TMPro;
using UnityEngine;

namespace UI
{
    internal sealed class PlayerHudView : MonoBehaviour, IPlayerHudView
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _coinsText;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void UpdateHealth(float value, float max) => _healthText.text = $"{value:F0}/{max:F0}";

        public void UpdateCoins(int value) => _coinsText.text = value.ToString();
    }
}