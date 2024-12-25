using System;
using Zenject;

namespace EndlessRoad
{
    public sealed class WaveCountPresenter : IInitializable, IDisposable
    {
        private readonly TextView _waveCountView;
        private readonly EventBus _bus;
        private int _waveCount;

        public WaveCountPresenter(TextView waveCountView, EventBus bus)
        {
            _waveCountView = waveCountView;
            _bus = bus;
        }

        private int WaveCount
        {
            get => _waveCount;
            set
            {
                value = UnityEngine.Mathf.Clamp(value, 0, value);
                _waveCount = value;
                _waveCountView.TextChange(_waveCount.ToString());
            }
        }

        public void SetWaveCount(int waveCount) => WaveCount = waveCount;

        public void Initialize()
        {
            _bus.WaveCleared += OnWaveCleared;
        }

        public void Dispose()
        {
            _bus.WaveCleared -= OnWaveCleared;
        }

        private void OnWaveCleared() => WaveCount--;

    }
}
