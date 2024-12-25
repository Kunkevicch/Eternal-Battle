using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class PlayerHealthPresenter : IInitializable, IDisposable
    {
        private readonly Health _playerHealth;
        private readonly ProgressFlowView _playerHealthView;

        private int _maxHealth;
        private int _currentHealth;

        public PlayerHealthPresenter(Health playerHealth, ProgressFlowView playerHealthView)
        {
            _playerHealth = playerHealth;
            _maxHealth = _playerHealth.MaxHealth;
            _currentHealth = _playerHealth.CurrentHealth;
            _playerHealthView = playerHealthView;
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
            _playerHealthView.SetProgress((float)_currentHealth / _maxHealth);
        }

    }
}
