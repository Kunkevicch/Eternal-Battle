using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class PlayerTakeDown
    {
        private readonly EventBus _eventBus;
        private readonly MonoBehaviour _player;

        private float _duration;

        private bool _isActive;
        private bool _isAvailable = true;
        private bool _isEnemyKilled;

        private Coroutine _secondChanceCoroutine;
        public PlayerTakeDown(MonoBehaviour monoBehaviour, EventBus eventBus, float duration)
        {
            _player = monoBehaviour;
            _eventBus = eventBus;
            _duration = duration;
        }

        public void ChangeDuration(float newDuration) => _duration = newDuration;

        public void Enable()
        {
            _eventBus.EnemyDied += OnEnemyDied;
            _eventBus.WaveCleared += OnWaveCleared;
        }

        public void Disable()
        {
            _eventBus.EnemyDied -= OnEnemyDied;
            _eventBus.WaveCleared -= OnWaveCleared;
        }

        private void OnEnemyDied(EnemyBase enemy)
        {
            if (!_isActive)
                return;
            _isEnemyKilled = true;
        }

        private void OnWaveCleared() => _isAvailable = true;

        public bool TryStartTakeDownRoutine(Action callback)
        {
            bool isAvailable = _isAvailable;
            if (_isAvailable)
            {
                _isActive = true;
                _isAvailable = false;
                _secondChanceCoroutine = _player.StartCoroutine(SecondChanceRoutine(callback));
            }

            return isAvailable;
        }

        public void CancelSecondChance()
        {
            if (_secondChanceCoroutine != null)
            {
                _player.StopCoroutine(_secondChanceCoroutine);
                _secondChanceCoroutine = null;
            }
        }

        private IEnumerator SecondChanceRoutine(Action callback)
        {
            float startTime = Time.time;
            float endTime = startTime + _duration;

            while (endTime > Time.time && !_isEnemyKilled)
            {
                yield return null;
            }

            if (_isEnemyKilled)
            {
                
                callback();
            }
            _isActive = false;
            _isEnemyKilled = false;
            _secondChanceCoroutine = null;
        }
    }
}
