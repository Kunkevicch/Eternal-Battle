using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int _maxHealth;
        [field: SerializeField] public ImpactEffect ImpactPrefab { get; set; }
        [SerializeField] protected int _currentHealth;

        private bool _isImmortal;
        private int _minimalHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public event Action<int> HealthChanged;
        public event Action Dead;

        public int MaxHealth => _maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth == value || _isImmortal)
                    return;

                _currentHealth = Mathf.Clamp(value, _minimalHealth, MaxHealth);

                HealthChanged?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                {
                    Dead?.Invoke();
                }
            }
        }

        public void ApplyDamage(int damage)
        {
            if (damage < 0)
                return;
            CurrentHealth -= damage;
        }

        public void Revive() => CurrentHealth = _maxHealth;

        public void SetMinimalHealth(int newMinimalHealth) => _minimalHealth = Mathf.Clamp(newMinimalHealth, 0, _maxHealth);

        public void StartImmortality(float duration) => StartCoroutine(ImmortalityRoutine(duration));

        private IEnumerator ImmortalityRoutine(float duration)
        {
            _isImmortal = true;
            yield return new WaitForSeconds(duration);
            _isImmortal = false;
        }
    }
}
