using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private EnemyWaveConfig _waves;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _player;
        [SerializeField] private int _count;
        private Dictionary<int, EnemyBase> _activeEnemies = new();

        private EventBus _eventBus;
        private ObjectPool _objectPool;

        private bool _isNeedNextWave = true;

        private Coroutine _spawnCoroutine;

        [Inject]
        public void Construct(EventBus eventBus, ObjectPool objectPool)
        {
            _eventBus = eventBus;
            _objectPool = objectPool;
        }

        public void StartSpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }

        private void OnEnable()
        {
            _eventBus.EnemyDied += OnEnemyDied;
        }

        private void OnDisable()
        {
            _eventBus.EnemyDied -= OnEnemyDied;
        }

        private IEnumerator SpawnEnemiesRoutine()
        {
            var currentWave = _waves.Waves[Random.Range(0, _waves.Waves.Count - 1)];
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
                        enemy.Revive();
                        enemyId++;
                        yield return null;
                    }
                }
                yield return null;
            }
            _count = _activeEnemies.Count;
        }

        private void OnEnemyDied(int id)
        {
            if (_activeEnemies.Count == 0)
                return;

            _activeEnemies.Remove(id);
            _count = _activeEnemies.Count;
            if (_activeEnemies.Count == 0)
            {
                StartCoroutine(SpawnEnemiesRoutine());
            }
        }
    }
}
