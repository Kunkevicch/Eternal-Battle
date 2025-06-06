using UnityEngine;

namespace EndlessRoad
{
    public interface IExplosion
    {
        public void InitializeExplosion(
          int damage
        , float damageRadius
        , int maxAffectedEnemies
        , LayerMask damageMask
        );

        public void ActivateExplosion();
    }
}