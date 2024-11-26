using UnityEngine;

namespace EndlessRoad
{
    public class ShootRaycastStrategySingle : ShootRaycastStrategyBase
    {
        public ShootRaycastStrategySingle(ObjectPool objectPool, AmmoBase shootTrail) : base(objectPool, shootTrail)
        {
        }

        public override void Shoot(
            Vector3 forwardDirection
            , Vector3 startPoint
            , float spread
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            )
        {
            IFireable instance = (IFireable)_objectPool.ReuseComponent(_ammo.gameObject, startPoint, Quaternion.LookRotation(forwardDirection));

            Vector3 direction = GetSpreadForProjectile(forwardDirection, spread);
            instance.ActivateAmmo();
            if (Physics.Raycast(
                   startPoint
                   , direction
                   , out RaycastHit hit
                   , 150f
                   , impactMask
                   ))
            {
                instance.StartLaunch(startPoint, hit.point, hit, simulationSpeed, duration, damage);
            }
            else
            {
                instance.StartLaunch(startPoint, startPoint + (direction * missDistance), new RaycastHit(), simulationSpeed, duration, damage);
            }
        }
    }
}
