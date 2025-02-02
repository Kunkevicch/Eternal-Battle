using UnityEngine;

namespace EndlessRoad
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _enemySpawnPoint;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public Transform EnemySpawnPoint => _enemySpawnPoint;
    }
}
