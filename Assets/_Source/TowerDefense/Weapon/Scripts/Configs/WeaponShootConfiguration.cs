using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "ShootConfiguration_", menuName = "Configs/Weapon/Shoot Configuration", order = 1)]
    public class WeaponShootConfiguration : ScriptableObject
    {
        public LayerMask ImpactMask;
        public float SpreadRadius;
        public float SpreadRadiusAiming;
        public float SpreadRadiusMove;

        public SpreadType SpreadType;
        public float MaxSpreadTime;
        public float FireRate;
        public float RecoilRecoverySpeed;

        public float GetSpread(float shootTime, bool isAim, bool isMove)
        {
            float spread = 0;

            if (isMove)
            {
                spread = SpreadRadiusMove;
            }

            if (SpreadType == SpreadType.Simple)
            {
                if (isAim)
                {
                    spread = SpreadRadiusAiming;
                }
                else
                {
                    spread = SpreadRadius;
                }
            }
            else if (SpreadType == SpreadType.TimeDependent)
            {
                spread = Mathf.Lerp(0, SpreadRadius, Mathf.Clamp01(shootTime / MaxSpreadTime));
            }

            return spread;
        }
    }
}
