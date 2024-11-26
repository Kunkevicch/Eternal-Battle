using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _weaponTransform;

        private bool _isPlayerWeapon;

        private Animator _animator;
        private WeaponStats _stats;
        private WeaponAmmo _weaponAmmo;

        private LayerMask _impactMask;
        private AudioSource _audioSource;
        private Vector3 _spawnPoint;
        private Vector3 _spawnRotation;
        private Vector3 _aimPoint;
        private Vector3 _aimRotation;

        private float _reloadTime;
        private float _fireRate;
        private int _damage;

        private ParticleSystem _muzzleParticle;
        private WeaponConfig _weaponConfiguration;
        private WeaponAudioConfig _audioConfig;

        private float _lastShootTime;
        private float _initialClickedTime;
        private float _stopShootingTime;

        private bool _isMove;
        private bool _isAim;
        private bool _isReloading;

        private ShootRaycastStrategyBase _shootStrategy;

        public void Initialize(
            LayerMask impactMask
            , WeaponConfig weaponConfig
            , ShootRaycastStrategyBase shootStrategy
            )
        {
            _stats = new();
            _weaponAmmo = new(weaponConfig.WeaponAmmoConfig.MaxAmmo, weaponConfig.WeaponAmmoConfig.ClipSize);

            _audioConfig = weaponConfig.WeaponAudioConfig;

            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponentInChildren<Animator>();
            _muzzleParticle = GetComponentInChildren<ParticleSystem>();

            _spawnPoint = weaponConfig.SpawnPoint;
            _spawnRotation = weaponConfig.SpawnRotation;

            _aimPoint = weaponConfig.AimPoint;
            _aimRotation = weaponConfig.AimRotation;

            _impactMask = impactMask;
            _weaponConfiguration = weaponConfig;
            _fireRate = weaponConfig.ShootConfiguration.FireRate;
            _reloadTime = weaponConfig.ReloadTime;
            _damage = weaponConfig.Damage;
            _shootStrategy = shootStrategy;
        }

        public event Action WeaponReloaded;

        public bool IsReloading => _isReloading;

        public WeaponAmmo WeaponAmmo => _weaponAmmo;

        public Vector3 SpawnPoint => _spawnPoint;
        public Vector3 SpawnRotation => _spawnRotation;

        public Vector3 AimPoint => _aimPoint;
        public Vector3 AimRotation => _aimRotation;

        public void SetShootMove(bool isMove) => _isMove = isMove;

        public void SetAimPosition(bool isAim)
        {
            _isAim = isAim;
            //StopCoroutine(SetAimPositionRoutine());
            //StartCoroutine(SetAimPositionRoutine());
        }

        //private IEnumerator SetAimPositionRoutine()
        //{
        //    float startTime = Time.time;
        //    Vector3 startPosition = transform.localPosition;

        //    if (_isAim)
        //    {
        //        float t = 0;
        //        while (t < 1f)
        //        {
        //            t = (Time.time - startTime) / 0.2f;
        //            transform.localPosition = Vector3.Lerp(startPosition, _aimPoint, t);
        //            yield return null;
        //        }
        //    }
        //    else
        //    {
        //        float t = 0;
        //        while (t < 1f)
        //        {
        //            t = (Time.time - startTime) / 0.2f;
        //            transform.localPosition = Vector3.Lerp(startPosition, _spawnPoint, t);
        //            yield return null;
        //        }
        //    }
        //}

        public void SetWeaponForPlayer(bool isPlayerWeapon)
        {
            _isPlayerWeapon = isPlayerWeapon;
        }

        private void OnEnable()
        {
            if (_isPlayerWeapon)
            {
                _audioConfig?.PlayEquipClip(_audioSource);
                //_animator?.Play("Equip");
            }
        }

        private void OnDisable()
        {
            if (_isPlayerWeapon)
            {
                //_weaponTransform.localEulerAngles = Vector3.zero;
                //_weaponTransform.localPosition = Vector3.zero;
                _isReloading = false;
            }
        }


        public void Tick(bool wantsToShoot, out bool canshoot)
        {
            if (wantsToShoot)
            {
                canshoot = CanShoot();
                Shoot();
            }
            else
            {
                canshoot = false;
                _stopShootingTime = Time.time;
            }
        }

        public Vector3 GetForwardDirection() =>
            _isPlayerWeapon ? Camera.main.transform.forward
            + Camera.main.transform.TransformDirection(new Vector3(0, -transform.rotation.x, 0))
            : _muzzleParticle.transform.forward;

        public float GetCurrentSpread() =>
            _weaponConfiguration.ShootConfiguration.GetSpread(
                Time.time - _initialClickedTime
                , _isAim
                , _isMove
                );

        public void StartReload()
        {
            if (!_weaponAmmo.CanReload() || _isReloading)
                return;
            _isReloading = true;
            StartCoroutine(ReloadRoutine());
        }


        private void Shoot()
        {
            if (_isReloading)
                return;

            if (Time.time - _lastShootTime - _fireRate > Time.deltaTime)
            {
                float lastDuration = Mathf.Clamp(
                    0
                    , (_stopShootingTime - _initialClickedTime)
                    , _weaponConfiguration.ShootConfiguration.MaxSpreadTime
                    );
                float lerpTime = (_weaponConfiguration.ShootConfiguration.RecoilRecoverySpeed - (Time.time - _stopShootingTime)) / _weaponConfiguration.ShootConfiguration.RecoilRecoverySpeed;
                _initialClickedTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
            }

            if (CanShoot())
            {
                _lastShootTime = Time.time;

                if (_weaponAmmo.IsEmpty())
                {
                    if (_isPlayerWeapon)
                    {
                        _audioConfig.PlayEmptyClip(_audioSource);
                    }

                    return;
                }

                _shootStrategy.Shoot(
                    GetForwardDirection()
                    , _muzzleParticle.transform.position
                    , GetCurrentSpread()
                    , _weaponConfiguration.TrailConfiguration.SimulationSpeed
                    , _weaponConfiguration.TrailConfiguration.Duration
                    , _damage
                    , _impactMask
                    , _weaponConfiguration.TrailConfiguration.MissDistance
                    );

                _audioConfig.PlayShootingClip(_audioSource);

                if (_isPlayerWeapon)
                {
                    //_animator?.Play("Fire");
                }

                _weaponAmmo.DecreaseAmmo();

                _muzzleParticle.Play();

            }
        }

        private bool CanShoot() => Time.time > _fireRate + _lastShootTime;

        private IEnumerator ReloadRoutine()
        {
            if (_isPlayerWeapon)
            {
                //_animator.Play("StartReload");
            }

            yield return new WaitForSeconds(_reloadTime);

            if (_isPlayerWeapon)
            {
                _audioConfig.PlayeReloadClip(_audioSource);
                while (_audioSource.isPlaying)
                {
                    yield return null;
                }
                //_animator.Play("EndReload");
            }
            _weaponAmmo.Reload();
            _isReloading = false;
            WeaponReloaded?.Invoke();
        }
    }
}
