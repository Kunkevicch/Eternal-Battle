using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class EnemyCombatBase : MonoBehaviour
    {
        protected WeaponView _currentWeapon;
        private WeaponViewIK _currentWeaponIK;

        private IWeaponFactory _weaponFactory;

        [Inject]
        public void Construct(IWeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
            Debug.Log(2222);
        }

        public WeaponViewIK Initialize(WeaponConfig weaponConfig)
        {
            _currentWeapon = _currentWeapon != null ? _currentWeapon : _weaponFactory.SpawnWeapon(weaponConfig, transform);
            _currentWeaponIK = _currentWeaponIK != null ? _currentWeaponIK : _currentWeapon.GetComponent<WeaponViewIK>();

            return _currentWeaponIK;
        }

        public WeaponViewIK Initialize(WeaponConfig weaponConfig, WeaponView weapon)
        {
            _currentWeapon = _currentWeapon != null ? _currentWeapon : _weaponFactory.InitializeSpawnedWeapon(weaponConfig, weapon);
            _currentWeaponIK = _currentWeaponIK != null ? _currentWeaponIK : _currentWeapon.GetComponent<WeaponViewIK>();

            return _currentWeaponIK;
        }

        public virtual void Attack()
        {
            _currentWeapon.Tick(true, out bool canshoot);
            if (!canshoot)
            {
                if (_currentWeapon.WeaponAmmo.IsEmpty() && _currentWeapon.WeaponAmmo.CanReload())
                {
                    _currentWeapon.StartReload();
                }
            }
        }
    }
}
