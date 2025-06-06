using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "PlayerConfigShooter", menuName = "Configs/Player/Shooter")]
    public class PlayerConfigShooter : ScriptableObject
    {
        public float SurvivalDuration;
    }
}
