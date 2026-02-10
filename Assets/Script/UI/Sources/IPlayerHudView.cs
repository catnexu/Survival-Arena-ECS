namespace UI
{
    public interface IPlayerHudView
    {
        void UpdateHealth(float value, float max);
        void UpdateCoins(int value);
        void SetActive(bool active);
    }
}