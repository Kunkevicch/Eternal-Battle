using System;
using Zenject;

namespace EndlessRoad
{
    public class SurvivalTimerPresenter : IInitializable, IDisposable
    {
        private readonly ProgressFlowView _survivalView;
        private readonly PlayerConfigShooter _playerConfig;
        private readonly EventBus _eventBus;

        private bool _isActive;

        public SurvivalTimerPresenter(EventBus eventBus, ProgressFlowView survivalView, PlayerConfigShooter playerConfig)
        {
            _eventBus = eventBus;
            _survivalView = survivalView;
            _playerConfig = playerConfig;
        }

        public void Initialize()
        {
            _eventBus.PlayerTakeDown += OnPlayerTakedDown;
            _eventBus.EnemyDied += OnEnemyDied;
            _eventBus.PlayerDead += OnPlayeDead;
        }

        public void Dispose()
        {
            _eventBus.PlayerTakeDown -= OnPlayerTakedDown;
            _eventBus.EnemyDied -= OnEnemyDied;
            _eventBus.PlayerDead -= OnPlayeDead;
        }

        private void OnPlayerTakedDown()
        {
            _isActive = true;
            _survivalView.gameObject.SetActive(true);
            _survivalView.SetProgress(0, _playerConfig.SurvivalDuration);
        }

        private void OnEnemyDied(EnemyBase enemy)
        {
            if (!_isActive)
                return;

            Deactivate();
        }

        private void OnPlayeDead()
        {
            Deactivate();
        }

        private void Deactivate()
        {
            _survivalView.ResetProgress();
            _isActive = false;
            _survivalView.gameObject.SetActive(false);
        }
    }
}
