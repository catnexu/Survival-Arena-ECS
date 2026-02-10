using System;
using ECS;

namespace Core
{
    internal sealed class PlayerController : IPlayerFactory, IGameEventsProvider
    {
        public event Action<int> OnGameOver;
        public event Action<PlayerData> OnNewUnitEvent;

        public void Start(PlayerConfig config)
        {
            OnNewUnitEvent?.Invoke(new PlayerData(config.Id, config.Stats, config.Weapons, config.Speed));
        }

        public void GameOver(int score) => OnGameOver?.Invoke(score);
    }
}