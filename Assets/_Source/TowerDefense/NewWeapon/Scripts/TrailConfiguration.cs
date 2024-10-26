using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "TrailConfiguration_", menuName = "Configs/Weapon/Trail Configuration", order = 2)]
    public class TrailConfiguration : ScriptableObject
    {
        public ShootTrail shootTrail;
        public Material Material;
        public AnimationCurve WidthCurve;
        public float Duration;
        public float MinVertexDistance;
        public Gradient Color;

        public float MissDistance;
        public float SimulationSpeed;
    }
}
