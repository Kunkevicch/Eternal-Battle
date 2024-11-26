using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "TrailConfiguration_", menuName = "Configs/Weapon/Trail Configuration", order = 2)]
    public class WeaponTrailConfiguration : ScriptableObject
    {
        public AmmoBase shootTrail;
        public float Duration;
        public float MissDistance;
        public float SimulationSpeed;
    }
}
