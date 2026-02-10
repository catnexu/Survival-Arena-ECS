using System;

namespace ECS
{
    public interface IPlayerFactory
    {
        event Action<PlayerData> OnNewUnitEvent;
    }
}