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

        private Coroutine _immortalCoroutine;
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
                if (value < _currentHealth && _isImmortal)
                    return;

                if (_currentHealth == value)
                    return;

                _currentHealth = Mathf.Clamp(value, _minimalHealth, MaxHealth);

                HealthChanged?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                {
                    Dead?.Invoke();
                }
            }
        }
        public float CurrentHealthPercent => (float)_currentHealth / _maxHealth;

        public void ApplyDamage(int damage)
        {
            if (damage < 0)
                return;
            CurrentHealth -= damage;
        }

        public void Revive() => CurrentHealth = _maxHealth;

        public void SetMinimalHealth(int newMinimalHealth) => _minimalHealth = Mathf.Clamp(newMinimalHealth, 0, _maxHealth);

        public void StartImmortality(float duration)
        {
            if (_immortalCoroutine != null)
            {
                StopCoroutine(_immortalCoroutine);
                _immortalCoroutine = null;
            }
            _immortalCoroutine = StartCoroutine(ImmortalityRoutine(duration));
        }
        private IEnumerator ImmortalityRoutine(float duration)
        {
            _isImmortal = true;
            yield return new WaitForSeconds(duration);
            _isImmortal = false;
        }
    }
}
