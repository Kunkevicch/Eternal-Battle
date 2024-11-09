using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "ShootConfiguration_", menuName = "Configs/Weapon/Shoot Configuration", order = 1)]
    public class WeaponShootConfiguration : ScriptableObject
    {
        public LayerMask ImpactMask;
        public Vector3 Spread = new(0.1f, 0.1f, 0.1f);
        public Vector3 SpreadAiming;
        public Vector3 SpreadMove;

        public SpreadType SpreadType;
        public float MaxSpreadTime;
        public float FireRate;
        public float RecoilRecoverySpeed;

        public Vector3 GetSpread(float shootTime, bool isAim, bool isMove)
        {
            Vector3 spread = Spread;

            if (isMove)
            {
                spread = SpreadMove;
            }

            if (SpreadType == SpreadType.Simple)
            {
                if (isAim)
                {
                    spread = SpreadAiming;
                }
                else
                {
                    spread = Spread;
                }
            }
            else if (SpreadType == SpreadType.TimeDependent)
            {
                spread = Vector3.Lerp(
                    Vector3.zero
                    , new Vector3(Random.Range(-Spread.x, Spread.x), Random.Range(-Spread.y, Spread.y))
                    , Mathf.Clamp01(shootTime / MaxSpreadTime)
                    );
            }

            return spread;
        }
    }
}
