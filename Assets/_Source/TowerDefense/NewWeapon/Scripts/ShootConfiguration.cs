using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "ShootConfiguration_", menuName = "Configs/Weapon/Shoot Configuration", order = 1)]
    public class ShootConfiguration : ScriptableObject
    {
        public LayerMask ImpactMask;
        public Vector3 Spread = new(0.1f, 0.1f, 0.1f);
        public Vector3 SpreadAiming;
        public Vector3 SpreadMove;
        public float FireRate;
    }
}
