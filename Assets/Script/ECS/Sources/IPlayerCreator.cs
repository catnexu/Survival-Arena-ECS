using System;

namespace ECS
{
    public interface IPlayerCreator
    {
        event Action<PlayerData> OnNewPlayerEvent;
    }
}