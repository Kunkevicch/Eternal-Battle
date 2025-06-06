using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Transform _handTransform;
        [SerializeField] private Transform _animatorTransform;
        [SerializeField] private PlayerWeaponHolderConfig _weaponHolderConfig;

        private List<WeaponView> _weapons = new();
        private Dictionary<WeaponView, AnimatorOverrideController> _animators = new();

        private bool _isAim;

        private WeaponHolderAnimator _animator;

        private WeaponSway _weaponSway;
        private WeaponRecoil _weaponRecoil;

        private WeaponView _activeWeapon;

        private CurrentAmmoInformer _currentAmmoInformer;

        public void InitializeWeapons()
        {
            _weaponHolderConfig.SpawnWeaponInHolder(this);
        }

        private void Awake()
        {
            _animator = new WeaponHolderAnimator(GetComponentInChildren<Animator>());
            _currentAmmoInformer = new();
            _weaponSway = GetComponent<WeaponSway>();
            _weaponRecoil = GetComponent<WeaponRecoil>();
        }

        public event Action WeaponChanged;
        public event Action<bool> WeaponShooted;

        public CurrentAmmoInformer CurrentAmmoInformer => _currentAmmoInformer;

        public WeaponView ActiveWeapon => _activeWeapon;
        public Transform Hand => _handTransform;

        public void AddWeapon(WeaponView newWeapon, AnimatorOverrideController animatorController = null)
        {
            if (_weapons.Contains(newWeapon) || !newWeapon)
                return;

            newWeapon.SetWeaponForPlayer(true);
            newWeapon.gameObject.SetActive(false);
            _weapons.Add(newWeapon);

            _animators[newWeapon] = animatorController;

            if (!_activeWeapon)
            {
                _activeWeapon = newWeapon;
                _activeWeapon.WeaponReloaded += OnWeaponReloaded;
                _activeWeapon.transform.SetParent(_animatorTransform);
                _animator.SetAnimatorController(_animators[_activeWeapon]);
                _handTransform.localPosition = _activeWeapon.SpawnPoint;
                _handTransform.localRotation = Quaternion.Euler(_activeWeapon.SpawnRotation);
                UpdateAmmoInformation();
            }

            _weaponSway.SetInitialPositionAndRotation(_handTransform.transform.localPosition, _handTransform.transform.localRotation);

            _weaponRecoil.SetHand(_handTransform);
            _activeWeapon.gameObject.SetActive(true);
        }

        public void SetWeapon(SetWeaponDirection direction)
        {
            if (_weapons.Count == 0)
                return;
            _activeWeapon.WeaponReloaded -= OnWeaponReloaded;
            _activeWeapon.gameObject.SetActive(false);
            _activeWeapon.transform.SetParent(transform);
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

            UpdateAmmoInformation();

            _activeWeapon.transform.SetParent(_animatorTransform);
            _activeWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _activeWeapon.gameObject.SetActive(true);
            _activeWeapon.WeaponReloaded += OnWeaponReloaded;

            _handTransform.localPosition = _activeWeapon.SpawnPoint;
            _handTransform.localRotation = Quaternion.Euler(_activeWeapon.SpawnRotation);

            _animator.SetAnimatorController(_animators[_activeWeapon]);
            _animator.PlayEquip();

            WeaponChanged?.Invoke();

            _weaponSway.SetInitialPositionAndRotation(_handTransform.localPosition, _handTransform.localRotation);
            _weaponRecoil.SetHand(_handTransform);
        }

        public void Sway(float X, float Y) => _weaponSway.SwayProcess(X, Y);

        public void Shoot(bool wantsToShoot)
        {
            if (_activeWeapon.IsReloading)
                return;

            if (_activeWeapon.WeaponAmmo.IsEmpty() && _activeWeapon.WeaponAmmo.CanReload())
            {
                Reload();
                WeaponShooted?.Invoke(false);
            }
            else
            {
                _activeWeapon.Tick(wantsToShoot, out bool canShoot);
                WeaponShooted?.Invoke(wantsToShoot);
                if (canShoot)
                {
                    if (wantsToShoot)
                    {
                        if (!_activeWeapon.WeaponAmmo.IsEmpty())
                        {
                            _weaponRecoil.RecoilProcess();
                        }
                        UpdateAmmoInformation();
                    }
                }
            }

        }

        public void Reload()
        {
            _activeWeapon.StartReload();
            _animator.PlayReloadStart();
        }

        public void Aim(bool aimStatus)
        {
            _activeWeapon?.SetAimPosition(aimStatus);
            SetAimPosition(aimStatus);
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

        private void SetAimPosition(bool isAim)
        {
            _isAim = isAim;
            StopCoroutine(SetAimPositionRoutine());
            StartCoroutine(SetAimPositionRoutine());
        }

        private IEnumerator SetAimPositionRoutine()
        {
            float startTime = Time.time;
            Vector3 startPosition = _handTransform.localPosition;

            if (_isAim)
            {
                float t = 0;
                while (t < 1f)
                {
                    t = (Time.time - startTime) / 0.2f;
                    _handTransform.localPosition = Vector3.Lerp(startPosition, _activeWeapon.AimPoint, t);
                    yield return null;
                }
            }
            else
            {
                float t = 0;
                while (t < 1f)
                {
                    t = (Time.time - startTime) / 0.2f;
                    _handTransform.localPosition = Vector3.Lerp(startPosition, _activeWeapon.SpawnPoint, t);
                    yield return null;
                }
            }
        }

        private void OnWeaponReloaded()
        {
            _animator.PlayReloadEnd();
            UpdateAmmoInformation();
        }

        private void UpdateAmmoInformation()
        {
            _currentAmmoInformer.CurrentAmmoInClip = _activeWeapon.WeaponAmmo.CurrentClipAmmo.ToString();
            _currentAmmoInformer.CurrentAmmo = _activeWeapon.WeaponAmmo.CurrentAmmo.ToString();
        }
    }
}
