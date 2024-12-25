using System;

namespace EndlessRoad
{
    public class EventBus
    {
        public event Action<EnemyBase> EnemyDied;
        public event Action LevelCompleted;
        public event Action WaveCleared;
        public event Action WaveReady;
        public event Action ActivateEnemy;
        public event Action<LevelDifficult> NeededNextWave;
        public event Action GameOver;
        public event Action GiveUp;
        public event Action SecondChance;
        public event Action PlayerInjured;
        public event Action PlayerRestored;

        public void RaiseEnemyDied(EnemyBase enemy) => EnemyDied?.Invoke(enemy);
        public void RaiseLevelCompleted() => LevelCompleted?.Invoke();
        public void RaiseWaveCleared() => WaveCleared?.Invoke();
        public void RaiseNeedNextWave(LevelDifficult levelDifficult) => NeededNextWave?.Invoke(levelDifficult);
        public void RaiseWaveReady() => WaveReady?.Invoke();
        public void RaiseActivateEnemy() => ActivateEnemy?.Invoke();
        public void RaiseGameOver() => GameOver?.Invoke();
        public void RaiseGiveUp() => GiveUp?.Invoke();
        public void RaiseSecondChance() => SecondChance?.Invoke();
        public void RaisePlayerInjured() => PlayerInjured?.Invoke();
        public void RaisePlayerRestored() => PlayerRestored?.Invoke();
    }
}
