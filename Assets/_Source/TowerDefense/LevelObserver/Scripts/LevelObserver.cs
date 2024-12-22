using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class LevelObserver : IInitializable, IDisposable
    {
        private LevelDifficult _levelDifficult;
        private int _waveCount;

        private EventBus _bus;
        public LevelObserver(EventBus bus)
        {
            _bus = bus;
        }
        public void InitializeLevel(LevelDifficult levelDifficult, int waveCount)
        {
            _levelDifficult = levelDifficult;
            _waveCount = waveCount;
        }

        public void Initialize()
        {
            _bus.WaveCleared += OnWaveCleared;
        }

        public void Dispose()
        {
            _bus.WaveCleared -= OnWaveCleared;
        }

        public void StartLevel() => _bus.RaiseNeedNextWave(_levelDifficult);

        private void OnWaveCleared()
        {
            _waveCount = Mathf.Clamp(_waveCount--, 0, _waveCount);
            if (_waveCount == 0)
            {
                _bus.RaiseLevelCompleted();
            }
            else
            {
                _bus.RaiseNeedNextWave(_levelDifficult);
            }
        }
    }
}
