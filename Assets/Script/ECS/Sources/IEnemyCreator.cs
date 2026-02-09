using System;

namespace ECS
{
    public interface IEnemyCreator
    {
        event Action<EnemyData> OnNewUnitEvent;
    }
}