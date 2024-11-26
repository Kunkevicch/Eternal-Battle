using UnityEngine;

namespace EndlessRoad
{
    public class ShootRaycastStrategyMultiple : ShootRaycastStrategyBase
    {
        private readonly int _projectileCount;

        public ShootRaycastStrategyMultiple(ObjectPool objectPool, AmmoBase ammo, int projectileCount) : base(objectPool, ammo)
        {
            _projectileCount = projectileCount;
        }

        public override void Shoot(
            Vector3 forwardDirection
            , Vector3 startPoint
            , float spreadRadius
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            )
        {

            for (int i = 0; i < _projectileCount; i++)
            {
                IFireable instance = (IFireable)_objectPool.ReuseComponent(_ammo.gameObject, startPoint, Quaternion.identity);
                Vector3 shootDirection = GetSpreadForProjectile(forwardDirection, spreadRadius);
                instance.ActivateAmmo();
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
