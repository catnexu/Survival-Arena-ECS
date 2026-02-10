using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Utilities;

namespace ECS
{
    internal sealed class SwordAttackSystem : IEcsRunSystem
    {
        private static Quaternion s_drawRotation = Quaternion.Euler(0f, 0f, 0f);
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventWorld = WorldNames.EVENT;

        private readonly EcsFilterInject<Inc<WeaponComponent, WeaponReadyFlag, WeaponMuzzleComponent, SwordComponent>, Exc<DestroyTag>>
            _weaponFilter = default;

        private readonly EcsFilterInject<Inc<TransformComponent, UnitHealthComponent, LayerComponent>, Exc<UnitDeadComponent>>
            _targetFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var weaponEntity in _weaponFilter.Value)
            {
                ref WeaponComponent weapon = ref _weaponFilter.Pools.Inc1.Get(weaponEntity);
                ref WeaponMuzzleComponent muzzle = ref _weaponFilter.Pools.Inc3.Get(weaponEntity);
                ref SwordComponent sword = ref _weaponFilter.Pools.Inc4.Get(weaponEntity);

                Vector3 attackPosition = muzzle.Value.position;
                float rangeSqr = sword.Range * sword.Range;

                foreach (var targetEntity in _targetFilter.Value)
                {
                    ref TransformComponent targetTransform = ref _targetFilter.Pools.Inc1.Get(targetEntity);
                    ref LayerComponent targetLayer = ref _targetFilter.Pools.Inc3.Get(targetEntity);

                    if (targetLayer.Value != weapon.TargetLayer)
                        continue;

                    Vector3 targetPos = targetTransform.Value.position;
                    float distanceSqr = (targetPos - attackPosition).sqrMagnitude;

                    if (distanceSqr <= rangeSqr)
                    {
                        ref DamageEvent damageEvent = ref _eventWorld.Value.SendEvent<DamageEvent>();
                        damageEvent.Damage = sword.Damage;
                        damageEvent.From = weapon.Owner;
                        damageEvent.To = _world.Value.PackEntity(targetEntity);
                    }
                }

                DrawInGame.Circle(attackPosition, s_drawRotation, sword.Range, sword.DrawColor, 0.5f);
                weapon.IsCharged = false;
                weapon.TimeSinceLastShot = 0f;
                _weaponFilter.Pools.Inc2.Del(weaponEntity);
            }
        }
    }
}