using System.Collections;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterEntryPoint : MonoBehaviour
    {
        private InitializingWeapons _weapons;
        private WeaponHolder _weaponHolder;
        private ObjectPool _objectPool;
        private EnemiesController _enemiesController;

        [Inject]
        public void Construct(
            InitializingWeapons weapons
            , ObjectPool objectPool
            , WeaponHolder weaponHolder
            , EnemiesController enemiesController
            )
        {
            _weapons = weapons;
            _objectPool = objectPool;
            _weaponHolder = weaponHolder;
            _enemiesController = enemiesController;
        }

        private IEnumerator Start()
        {
            foreach (var weapon in _weapons.WeaponConfigs)
            {
                weapon.SetObjectPool(_objectPool);
                yield return null;
            }
            yield return null;
            _weaponHolder.InitializeWeapons();
            _enemiesController.StartSpawnEnemies();
        }
    }
}
