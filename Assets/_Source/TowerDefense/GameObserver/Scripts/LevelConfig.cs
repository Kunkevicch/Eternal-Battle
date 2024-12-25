using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Configs/Level")]
    public class LevelConfig : ScriptableObject
    {
        public LevelDifficult levelDifficult;
        public int WaveCount;
    }
}
