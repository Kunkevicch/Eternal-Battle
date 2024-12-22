using System;

namespace EndlessRoad
{
    public class EventBus
    {
        public event Action<EnemyBase> EnemyDied;
        public event Action LevelCompleted;
        public event Action WaveCleared;
        public event Action<LevelDifficult> NeededNextWave;
        public void RaiseEnemyDied(EnemyBase enemy) => EnemyDied?.Invoke(enemy);
        public void RaiseLevelCompleted() => LevelCompleted?.Invoke();
        public void RaiseWaveCleared() => WaveCleared?.Invoke();
        public void RaiseNeedNextWave(LevelDifficult levelDifficult) => NeededNextWave?.Invoke(levelDifficult);
    }
}
