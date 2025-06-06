using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class PlayerHealthPresenter : IInitializable, IDisposable
    {
        private readonly Health _playerHealth;
        private readonly ProgressFlowView _playerHealthView;
        private readonly TextView _playerHealthText;

        private int _maxHealth;
        private int _currentHealth;

        public PlayerHealthPresenter(Health playerHealth, ProgressFlowView playerHealthView, TextView playerHealthText)
        {
            _playerHealth = playerHealth;
            _maxHealth = _playerHealth.MaxHealth;
            _currentHealth = _playerHealth.CurrentHealth;
            _playerHealthView = playerHealthView;
            _playerHealthText = playerHealthText;
        }

        public void Initialize()
        {
            _playerHealth.HealthChanged += OnHealthChanged;
        }
        public void Dispose()
        {
            _playerHealth.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int newHealth)
        {
            _currentHealth = newHealth;
            _playerHealthView.SetProgress(_playerHealth.CurrentHealthPercent);
            _playerHealthText.TextChange(Mathf.CeilToInt(_playerHealth.CurrentHealthPercent * 100).ToString() + "%");
        }

    }
}
