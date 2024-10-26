using System;
using UnityEngine;

namespace EndlessRoad
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int _maxHealth;
        [field: SerializeField] public ImpactEffect ImpactPrefab { get; set; }
        protected int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public event Action Dead;

        public int MaxHealth => _maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            }
        }


        public void ApplyDamage(int damage)
        {
            if (damage < 0)
                return;
            CurrentHealth -= damage;
            Dead?.Invoke();
        }
    }
}
