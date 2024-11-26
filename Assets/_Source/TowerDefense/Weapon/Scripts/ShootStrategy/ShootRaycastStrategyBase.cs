using UnityEngine;

namespace EndlessRoad
{
    public abstract class ShootRaycastStrategyBase
    {
        protected readonly ObjectPool _objectPool;
        protected readonly AmmoBase _ammo;

        public ShootRaycastStrategyBase(ObjectPool objectPool, AmmoBase ammo)
        {
            _objectPool = objectPool;
            _ammo = ammo;
        }

        protected Vector3 GetSpreadForProjectile(Vector3 direction, float spreadRadius)
        {
            return direction + Random.insideUnitSphere * spreadRadius;
        }

        public abstract void Shoot(
            Vector3 shootDirection
            , Vector3 startPoint
            , float spread
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            );
    }
}
