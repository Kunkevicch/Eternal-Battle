using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "EnemyWeapon_", menuName = "Configs/Weapon/Enemy Weapon", order = 6)]
    public class EnemyWeaponConfig : WeaponConfig
    {
        public Vector3 RightHandTargetPosition;
        public Vector3 LeftHandTargetPosition;

    }
}
