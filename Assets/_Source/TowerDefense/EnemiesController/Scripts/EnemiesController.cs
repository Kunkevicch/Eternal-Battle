using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{

    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private EnemyWaveConfig _waves;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _player;

        private Health _playerHealth;
        private Dictionary<int, EnemyBase> _activeEnemies = new();

        private EventBus _eventBus;
        private ObjectPool _objectPool;

        [Inject]
        public void Construct(EventBus eventBus, ObjectPool objectPool)
        {
            _eventBus = eventBus;
            _objectPool = objectPool;
        }

        private void Awake()
        {
            _playerHealth = _player.GetComponent<Health>();
        }

        private void OnEnable()
        {
            _eventBus.EnemyDied += OnEnemyDied;
            _eventBus.NeededNextWave += OnNeededNextWave;
            _eventBus.ActivateEnemy += OnActivateEnemy;
            _eventBus.GameOver += OnGameOver;
            _eventBus.SecondChance += OnActivateEnemy;
        }

        private void OnDisable()
        {
            _eventBus.EnemyDied -= OnEnemyDied;
            _eventBus.NeededNextWave -= OnNeededNextWave;
            _eventBus.ActivateEnemy -= OnActivateEnemy;
            _eventBus.GameOver -= OnGameOver;
            _eventBus.SecondChance -= OnActivateEnemy;
        }

        private void OnGameOver()
        {
            if (_activeEnemies.Count > 0)
            {
                foreach (var enemy in _activeEnemies)
                {
                    enemy.Value.Deactivate();
                }
            }
        }

        private void OnNeededNextWave(LevelDifficult levelDifficult)
        {
            ClearWave();
            StartCoroutine(SpawnEnemiesRoutine(levelDifficult));
        }

        private IEnumerator SpawnEnemiesRoutine(LevelDifficult levelDifficult)
        {
            yield return new WaitForSeconds(5f);
            var currentWave = GetWaveByLevelDifficult(levelDifficult);
            int enemyId = 0;
            while (_activeEnemies.Count < currentWave.EnemiesCount)
            {
                foreach (var keyValuePairWave in currentWave.Enemies)
                {
                    for (int enemyIterator = 0; enemyIterator < keyValuePairWave.Count; enemyIterator++)
                    {
                        EnemyBase enemy = (EnemyBase)_objectPool.ReuseComponent(
                            keyValuePairWave.Enemy.gameObject
                            , _spawnPoint.position
                            , Quaternion.identity
                            );

                        _activeEnemies.Add(enemyId, enemy);
                        enemy.SetID(enemyId);
                        enemy.SetPlayer(_player);
                        enemy.gameObject.SetActive(true);
                        enemy.Initialize();
                        enemy.Deactivate();
                        enemyId++;
                        yield return null;
                    }
                }
                yield return null;
            }
            _eventBus.RaiseWaveReady();
        }

        private void ClearWave()
        {
            if (_activeEnemies.Count == 0)
                return;

            foreach (var enemy in _activeEnemies)
            {
                enemy.Value.Clear();
            }

            _activeEnemies.Clear();
        }


        private Wave GetWaveByLevelDifficult(LevelDifficult levelDifficult)
        {
            var wavesByDifficult = _waves.Waves.Where(x => x.WaveDifficult == levelDifficult).ToList();
            return wavesByDifficult[UnityEngine.Random.Range(0, wavesByDifficult.Count)];
        }

        private void OnEnemyDied(EnemyBase enemy)
        {
            if (_activeEnemies.Count == 0)
                return;

            _activeEnemies.Remove(enemy.ID);
            if (_activeEnemies.Count == 0)
            {
                _eventBus.RaiseWaveCleared();
            }
        }

        private void OnActivateEnemy()
        {
            if (_activeEnemies.Count > 0)
            {
                foreach (var enemy in _activeEnemies)
                {
                    enemy.Value.Activate();
                }
            }
            else
            {
                _eventBus.RaiseWaveCleared();
            }
        }

    }
}