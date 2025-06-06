using UnityEngine;

namespace EndlessRoad
{
    public class ShootStrategyDecorator
    {
        private ShootRaycastStrategyBase _currentStrategy;

        public void SetShootStrategy(ShootRaycastStrategyBase shootRayCastStrategy)
        {
            _currentStrategy = shootRayCastStrategy;
        }

        public void Shoot(
             Vector3 forwardDirection
            , Vector3 startPoint
            , float spreadRadius
            , float simulationSpeed
            , float duration
            , int damage
            , LayerMask impactMask
            , float missDistance
            ) => _currentStrategy?.Shoot(
                forwardDirection
                , startPoint
                , spreadRadius
                , simulationSpeed
                , duration
                , damage
                , impactMask
                , missDistance
                );
    }
}
