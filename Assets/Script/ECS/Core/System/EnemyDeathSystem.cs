using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS
{
    internal sealed class EnemyDeathSystem : IEcsPostRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent, EnemyTag, UnitDeadComponent>, Exc<DestroyTag>> _deadFilter = default;
        private readonly EcsPoolInject<DestroyTag> _destroyPool = default;
        public void PostRun(IEcsSystems systems)
        {
            foreach (var i in _deadFilter.Value)
            {
                _destroyPool.Value.Add(i);
            }
        }
    }
}