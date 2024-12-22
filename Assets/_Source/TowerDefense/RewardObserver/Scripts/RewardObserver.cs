using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class RewardObserver : IInitializable, IDisposable
    {
        private int _currentReward;

        private EventBus _bus;

        public RewardObserver(EventBus bus)
        {
            _bus = bus;
        }

        public int CurrentReward
        {
            get => _currentReward;

            private set
            {
                value = Math.Clamp(value, 0, value);
                if (value != _currentReward)
                {
                    _currentReward = value;
                    RewardChanged?.Invoke(_currentReward);
                }
            }
        }

        public Action<int> RewardChanged;

        public void Initialize()
        {
            _bus.EnemyDied += OnEnemyDied;
        }
        public void Dispose()
        {
            _bus.EnemyDied -= OnEnemyDied;
        }

        private void OnEnemyDied(EnemyBase enemyBase)
        {
            CurrentReward += enemyBase.Reward;
        }
    }
}
