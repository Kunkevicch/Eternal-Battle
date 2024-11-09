using UnityEngine;

namespace EndlessRoad
{
    public abstract class ShootRaycastStrategyBase
    {
        protected readonly ObjectPool _objectPool;
        protected readonly ShootTrail _shootTrail;

        public ShootRaycastStrategyBase(ObjectPool objectPool, ShootTrail shootTrail)
        {
            _objectPool = objectPool;
            _shootTrail = shootTrail;
        }

        protected Vector3 GetSpreadForProjectile(Vector3 direction, Vector3 spread)
        {
            return direction + new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y));
        }

        public abstract void Shoot(
            Vector3 shootDirection
            , Vector3 startPoint
            , Vector3 spread
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            );
    }
}
