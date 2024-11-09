using UnityEngine;

namespace EndlessRoad
{
    public class ShootRaycastStrategySingle : ShootRaycastStrategyBase
    {
        public ShootRaycastStrategySingle(ObjectPool objectPool, ShootTrail shootTrail) : base(objectPool, shootTrail)
        {
        }

        public override void Shoot(
            Vector3 forwardDirection
            , Vector3 startPoint
            , Vector3 spread
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            )
        {
            ShootTrail instance = (ShootTrail)_objectPool.ReuseComponent(_shootTrail.gameObject, startPoint, Quaternion.identity);
            Vector3 direction = GetSpreadForProjectile(forwardDirection, spread);
            instance.gameObject.SetActive(true);
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
