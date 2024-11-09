using System;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    public class WeaponHolder : MonoBehaviour
    {
        private List<WeaponView> _weapons = new();

        private WeaponSway _weaponSway;
        private WeaponRecoil _weaponRecoil;

        private WeaponView _activeWeapon;

        private CurrentAmmoInformer _currentAmmoInformer;

        private void Awake()
        {
            _currentAmmoInformer = new();
            _weaponSway = GetComponent<WeaponSway>();
            _weaponRecoil = GetComponent<WeaponRecoil>();
        }

        public CurrentAmmoInformer CurrentAmmoInformer => _currentAmmoInformer;

        public WeaponView ActiveWeapon => _activeWeapon;

        public void AddWeapon(WeaponView newWeapon, AnimatorOverrideController animatorController = null)
        {
            if (_weapons.Contains(newWeapon) || !newWeapon)
                return;

            newWeapon.SetWeaponForPlayer(true);
            newWeapon.gameObject.SetActive(false);
            _weapons.Add(newWeapon);

            _activeWeapon ??= newWeapon;

            _weaponSway.SetInitialPositionAndRotation(_activeWeapon.transform.localPosition, _activeWeapon.transform.localRotation);

            _weaponRecoil.SetHand(_activeWeapon.transform);
            _activeWeapon.gameObject.SetActive(true);
        }

        public void SetWeapon(SetWeaponDirection direction)
        {
            if (_weapons.Count == 0)
                return;

            _activeWeapon.gameObject.SetActive(false);
            int activeWeaponIndex = _weapons.IndexOf(_activeWeapon);

            if (direction == SetWeaponDirection.Next)
            {
                if (activeWeaponIndex == _weapons.Count - 1)
                {
                    _activeWeapon = _weapons[0];
                }
                else
                {
                    _activeWeapon = _weapons[++activeWeaponIndex];
                }
            }
            else
            {
                if (activeWeaponIndex == 0)
                {
                    _activeWeapon = _weapons[_weapons.Count - 1];
                }
                else
                {
                    _activeWeapon = _weapons[--activeWeaponIndex];
                }
            }

            _currentAmmoInformer.CurrentAmmoInClip = _activeWeapon.WeaponAmmo.CurrentClipAmmo.ToString();
            _currentAmmoInformer.CurrentAmmo = _activeWeapon.WeaponAmmo.CurrentAmmo.ToString();

            _activeWeapon.gameObject.SetActive(true);

            _weaponSway.SetInitialPositionAndRotation(_activeWeapon.transform.localPosition, _activeWeapon.transform.localRotation);
            _weaponRecoil.SetHand(_activeWeapon.transform);
        }

        public void Sway(float X, float Y) => _weaponSway.SwayProcess(X, Y);

        public void Shoot(bool wantsToShoot)
        {
            _activeWeapon.Tick(wantsToShoot);
            if (wantsToShoot)
            {
                if (!_activeWeapon.WeaponAmmo.IsEmpty())
                {
                    _weaponRecoil.RecoilProcess();
                }
                _currentAmmoInformer.CurrentAmmoInClip = _activeWeapon.WeaponAmmo.CurrentClipAmmo.ToString();
                _currentAmmoInformer.CurrentAmmo = _activeWeapon.WeaponAmmo.CurrentAmmo.ToString();
            }
        }

        public void Reload()
        {
            _activeWeapon.StartReload();
        }

        public void Aim(bool aimStatus)
        {
            _activeWeapon?.SetAimPosition(aimStatus);

            if (aimStatus)
            {
                _weaponSway.SetInitialPositionAndRotation(_activeWeapon.AimPoint, Quaternion.Euler(_activeWeapon.AimRotation));
            }
            else
            {
                _weaponSway.SetInitialPositionAndRotation(_activeWeapon.SpawnPoint, Quaternion.Euler(_activeWeapon.SpawnRotation));
            }

            _weaponRecoil.SetAimState(aimStatus);
        }
    }

    public class CurrentAmmoInformer
    {
        private string _currentAmmoInClip;
        private string _currentAmmo;

        public string CurrentAmmoInClip
        {
            get => _currentAmmoInClip;

            set
            {
                _currentAmmoInClip = value;
                AmmoInClipChanged?.Invoke(value);
            }
        }

        public string CurrentAmmo
        {
            get => _currentAmmo;

            set
            {
                _currentAmmo = value;
                CurrentAmmoChanged?.Invoke(value);
            }
        }

        public event Action<string> CurrentAmmoChanged;
        public event Action<string> AmmoInClipChanged;
    }
}
