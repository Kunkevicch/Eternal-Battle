using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class GameObserver : IInitializable, IDisposable
    {
        // TODO: swap to player
        // TODO: CHANGE EVENTBUS, MAKE HIM MORE FLEXIBLE AND EXPANDABLE
        private readonly EventBus _eventBus;

        private LevelDifficult _levelDifficult;
        private int _waveCount;
        private int _livesCount;

        public GameObserver(EventBus bus)
        {
            _eventBus = bus;
        }

        public int LivesCount => _livesCount;

        public void InitializeLevel(LevelDifficult levelDifficult, int waveCount, int livesCount = 1)
        {
            _levelDifficult = levelDifficult;
            _waveCount = waveCount;
            _livesCount = livesCount;
        }

        public void Initialize()
        {
            _eventBus.WaveCleared += OnWaveCleared;
            _eventBus.WaveReady += OnWaveReady;
            _eventBus.PlayerDead += OnPlayerDead;
            _eventBus.SecondChance += OnSecondChance;
        }

        public void Dispose()
        {
            _eventBus.WaveCleared -= OnWaveCleared;
            _eventBus.WaveReady -= OnWaveReady;
            _eventBus.PlayerDead -= OnPlayerDead;
            _eventBus.SecondChance -= OnSecondChance;
        }

        public void StartLevel() => _eventBus.RaiseNeedNextWave(_levelDifficult);

        private void OnWaveCleared()
        {
            _waveCount = Mathf.Clamp(_waveCount--, 0, _waveCount);
            if (_waveCount == 0)
            {
                _eventBus.RaiseLevelCompleted();
            }
            else
            {
                _eventBus.RaiseNeedNextWave(_levelDifficult);
            }
        }

        private void OnWaveReady()
        {
            // add waiting for player choose buff
            _eventBus.RaiseActivateEnemy();
        }

        private void OnPlayerDead()
        {
            _eventBus.RaiseGameOver();
        }
        private void OnSecondChance()
        {
            _livesCount--;
        }
    }
}
