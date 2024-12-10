using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "EnemyWave_", menuName = "Configs/Enemy/Wave")]
    public class EnemyWaveConfig : ScriptableObject
    {
        [SerializeField] private List<Wave> _waves;
        public List<Wave> Waves => _waves;
    }
}
