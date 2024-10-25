using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private LayerMask _impactLayer;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _spreadRadius;
        [SerializeField] private int _damage;
        [SerializeField] private float _ammoDistance;
        [SerializeField] private Transform _fpsCamera;

        private int _currentAmmo;
        private int _currentAmmoClip;
        private int _maxAmmoInClip;
        private RaycastHit _rayHit;

        private Vector3 ScreenCenter => new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        private Vector3 TargetPoint => Camera.main.ScreenToWorldPoint(new Vector3(ScreenCenter.x, ScreenCenter.y, Camera.main.farClipPlane));

        private ImpactService _impactService;

        [Inject]
        private void Construct(ImpactService impactService)
        {
            _impactService = impactService;
        }

        public event Action WeaponReloaded;

        public int CurrentAmmo
        {
            get => _currentAmmo;

            set => _currentAmmo = Mathf.Clamp(value, 0, value);
        }

        public int CurrentAmmoClip
        {
            get => _currentAmmoClip;

            set => _currentAmmoClip = Mathf.Clamp(value, 0, _maxAmmoInClip);
        }
        public float FireRate => _fireRate;

        public void Fire()
        {
            var direction = GetSpreadPoint();
            if (Physics.Raycast(_shootPoint.position, direction, out _rayHit, _ammoDistance, _impactLayer))
            {
                if (_rayHit.collider.TryGetComponent(out IImpactable impactable))
                {
                    _impactService.DoImpactToTargetPoint(impactable, _rayHit.point);
                    if (impactable is IDamageable damageable)
                    {
                        damageable.ApplyDamage(_damage);
                    }
                }
            }
        }

        private Vector3 GetSpreadPoint()
        {
            Vector3 randomSpread = UnityEngine.Random.insideUnitCircle * _spreadRadius;
            Vector3 finalTarget = TargetPoint + randomSpread;
            return (finalTarget - _shootPoint.position);
        }

        public void Reload()
        {
            if (CurrentAmmo == 0)
            {
                //событие - патронов нет
                return;
            }

            CurrentAmmo = _maxAmmoInClip;

            WeaponReloaded?.Invoke();
        }
    }
}
