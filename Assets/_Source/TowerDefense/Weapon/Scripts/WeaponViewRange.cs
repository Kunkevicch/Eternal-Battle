using UnityEngine;

namespace EndlessRoad
{
    public class WeaponViewRange : WeaponView
    {
        private void OnDisable()
        {
            if (_isPlayerWeapon)
            {
                _isReloading = false;
            }
        }
        public override void Initialize(LayerMask impactMask, WeaponConfig weaponConfig, ShootRaycastStrategyBase shootStrategy)
        {
            _weaponAmmo = new(weaponConfig.WeaponAmmoConfig.MaxAmmo, weaponConfig.WeaponAmmoConfig.ClipSize, _isPlayerWeapon);

            _audioConfig = weaponConfig.WeaponAudioConfig;

            _audioSource = GetComponent<AudioSource>();
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

        protected override void Attack()
        {
            if (_isReloading)
                return;

            if (Time.time - _lastAttackTime - _fireRate > Time.deltaTime)
            {
                float lastDuration = Mathf.Clamp(
                    0
                    , (_stopAttackTime - _initialClickedTime)
                    , _weaponConfiguration.ShootConfiguration.MaxSpreadTime
                    );
                float lerpTime = (_weaponConfiguration.ShootConfiguration.RecoilRecoverySpeed - (Time.time - _stopAttackTime)) / _weaponConfiguration.ShootConfiguration.RecoilRecoverySpeed;
                _initialClickedTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
            }

            if (CanAttack())
            {
                _lastAttackTime = Time.time;

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
    }
}
