using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public abstract class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _weaponTransform;

        protected bool _isPlayerWeapon;
        protected WeaponAmmo _weaponAmmo;

        protected LayerMask _impactMask;
        protected AudioSource _audioSource;
        protected Vector3 _spawnPoint;
        protected Vector3 _spawnRotation;
        protected Vector3 _aimPoint;
        protected Vector3 _aimRotation;

        protected float _reloadTime;
        protected float _fireRate;
        protected int _damage;

        protected WeaponConfig _weaponConfiguration;
        protected WeaponAudioConfig _audioConfig;
        protected ParticleSystem _muzzleParticle;

        protected float _lastAttackTime;
        protected float _initialClickedTime;
        protected float _stopAttackTime;

        protected bool _isMove;
        protected bool _isAim;
        protected bool _isReloading;

        protected ShootRaycastStrategyBase _shootStrategy;

        

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
        }

        public void SetWeaponForPlayer(bool isPlayerWeapon)
        {
            _isPlayerWeapon = isPlayerWeapon;
        }

        private void OnEnable()
        {
            if (_isPlayerWeapon)
            {
                _audioConfig?.PlayEquipClip(_audioSource);
            }
        }

        public void Tick(bool wantsToAttack, out bool canAttack)
        {
            if (wantsToAttack)
            {
                canAttack = CanAttack();
                Attack();
            }
            else
            {
                canAttack = false;
                _stopAttackTime = Time.time;
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


        
        protected bool CanAttack() => Time.time > _fireRate + _lastAttackTime;

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

        public abstract void Initialize(
            LayerMask impactMask
            , WeaponConfig weaponConfig
            , ShootRaycastStrategyBase shootStrategy
            );
        protected abstract void Attack();
    }

}
