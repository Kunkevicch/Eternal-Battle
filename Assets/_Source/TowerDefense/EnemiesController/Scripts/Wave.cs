using System;
using System.Collections.Generic;

namespace EndlessRoad
{
    [Serializable]
    public struct Wave
    {
        public LevelDifficult WaveDifficult;
        public List<WaveKeyPair> Enemies;
        public int EnemiesCount
        {
            get
            {
                int count = 0;
                
                foreach (var waveKeyPair in Enemies)
                {
                    count += waveKeyPair.Count;
                }

                return count;
            }
        }
    }

    [Serializable]
    public struct WaveKeyPair
    {
        public int Count;
        public EnemyBase Enemy;
    }
}
