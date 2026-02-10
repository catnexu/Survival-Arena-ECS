using System;
using ECS;

namespace Core
{
    internal sealed class PlayerController : IPlayerFactory
    {
        public event Action<PlayerData> OnNewUnitEvent;
        
        public void Start(PlayerConfig config)
        {
            OnNewUnitEvent?.Invoke(new PlayerData(config.Id, config.Stats, config.Weapons, config.Speed));
        }
    }
}