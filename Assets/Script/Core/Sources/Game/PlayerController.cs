using System;
using ECS;

namespace Core
{
    internal sealed class PlayerController : IPlayerCreator
    {
        public event Action<PlayerData> OnNewPlayerEvent;
        
        public void Start(PlayerConfig config)
        {
            OnNewPlayerEvent?.Invoke(new PlayerData(config.Id, config.Stats));
        }
    }
}