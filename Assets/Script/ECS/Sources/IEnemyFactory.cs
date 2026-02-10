using System;

namespace ECS
{
    public interface IEnemyFactory
    {
        event Action<EnemyData> OnNewUnitEvent;
    }
}