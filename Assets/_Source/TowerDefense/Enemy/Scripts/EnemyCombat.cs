using UnityEngine;

namespace EndlessRoad
{
    public class EnemyCombat : MonoBehaviour
    {
        private WeaponView _currentWeapon;
        private WeaponViewIK _currentWeaponIK;

        public WeaponViewIK Initialize(WeaponConfig weaponConfig)
        {
            _currentWeapon ??= weaponConfig.Spawn(transform);
            _currentWeaponIK ??= _currentWeapon.GetComponent<WeaponViewIK>();

            return _currentWeaponIK;
        }

        public void Attack()
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
