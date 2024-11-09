using UnityEngine;

namespace EndlessRoad
{
    public class ShootRaycastStrategyMultiple : ShootRaycastStrategyBase
    {
        private readonly int _projectileCount;

        public ShootRaycastStrategyMultiple(ObjectPool objectPool, ShootTrail shootTrail, int projectileCount) : base(objectPool, shootTrail)
        {
            _projectileCount = projectileCount;
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

            for (int i = 0; i < _projectileCount; i++)
            {
                ShootTrail instance = (ShootTrail)_objectPool.ReuseComponent(_shootTrail.gameObject, startPoint, Quaternion.identity);
                //Random.InitState(i + Time.frameCount);
                Vector3 shootDirection = GetSpreadForProjectile(forwardDirection, spread);
                instance.gameObject.SetActive(true);
                if (Physics.Raycast(
                   startPoint
                   , shootDirection
                   , out RaycastHit hit
                   , 150f
                   , impactMask
                   ))
                {
                    instance.StartLaunch(startPoint, hit.point, hit, simulationSpeed, duration, damage);
                }
                else
                {
                    instance.StartLaunch(startPoint, startPoint + (shootDirection * missDistance), new RaycastHit(), simulationSpeed, duration, damage);
                }
            }
        }
    }
}
