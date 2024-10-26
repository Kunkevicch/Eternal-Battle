using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class WeaponView : MonoBehaviour
    {
        private LayerMask _impactMask;

        private Vector3 _spread;
        private Vector3 _spreadMove;
        private Vector3 _spreadAim;

        private float _fireRate;
        private int _damage;

        private ShootTrail _shotTrail;
        private ParticleSystem _muzzleParticle;
        private TrailConfiguration _trailConfiguration;

        private float _lastShootTime;
        private bool _isMove;

        private ObjectPool _objectPool;
        private ImpactService _impactService;

        public void Initialize(
            LayerMask impactMask
            , int damage
            , TrailConfiguration trailConfiguration
            , ShootConfiguration shootConfiguration
            , ObjectPool objectPool
            , ImpactService impactService
            )
        {
            _muzzleParticle = GetComponentInChildren<ParticleSystem>();
            _impactMask = impactMask;
            _spread = shootConfiguration.Spread;
            _spreadMove = shootConfiguration.SpreadMove;
            _fireRate = shootConfiguration.FireRate;
            _damage = damage;
            _trailConfiguration = trailConfiguration;
            _shotTrail = trailConfiguration.shootTrail;
            _objectPool = objectPool;
            _impactService = impactService;
        }

        public void SetShootMove(bool isMove) => _isMove = isMove;

        public void Shoot()
        {
            if (Time.time > _fireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;

                Vector3 shootDirection = GetSpread();

                //shootDirection.Normalize();

                if (Physics.Raycast(
                   _muzzleParticle.transform.position
                   , shootDirection
                   , out RaycastHit hit
                   , float.MaxValue
                   , _impactMask
                   ))
                {
                    StartCoroutine(LaunchTrail(
                        _muzzleParticle.transform.position
                        , hit.point
                        , hit
                        ));
                }
                else
                {
                    StartCoroutine(LaunchTrail(
                     _muzzleParticle.transform.position
                     , _muzzleParticle.transform.position + (shootDirection * _trailConfiguration.MissDistance)
                     , new RaycastHit()
                     ));
                }

            }
        }

        private Vector3 GetSpread()
        {
            Vector3 spread;

            if (_isMove)
            {
                spread = _muzzleParticle.transform.forward + new Vector3(Random.Range(-_spreadMove.x, _spreadMove.x), Random.Range(-_spreadMove.y, _spreadMove.y));
            }
            else
            {
                spread = _muzzleParticle.transform.forward + new Vector3(Random.Range(-_spread.x, _spread.x), Random.Range(-_spread.y, _spread.y));
            }

            return spread;
        }

        private IEnumerator LaunchTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
        {
            ShootTrail instance = (ShootTrail)_objectPool.ReuseComponent(_shotTrail.gameObject, startPoint, Quaternion.identity);

            Transform instanceParent = instance.transform.parent;
            instance.Trail.gameObject.SetActive(true);
            InitializeTrail(instance.Trail);
            instance.transform.SetParent(_muzzleParticle.transform);
            instance.transform.localPosition = Vector3.zero;

            yield return null;

            instance.Trail.emitting = true;

            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;

            _muzzleParticle.Play();

            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    startPoint
                    , endPoint
                    , Mathf.Clamp01(1 - (remainingDistance / distance))
                    );
                remainingDistance -= _trailConfiguration.SimulationSpeed * Time.deltaTime;
                yield return null;
            }

            instance.transform.position = endPoint;

            yield return new WaitForSeconds(_trailConfiguration.Duration);

            yield return null;

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IImpactable impactable))
                {
                    _impactService.DoImpactToTargetPoint(impactable, hit.point);
                    if (impactable is IDamageable damageable)
                    {
                        damageable.ApplyDamage(_damage);
                    }
                }
            }

            instance.Trail.emitting = false;
            instance.transform.SetParent(instanceParent);
            instance.gameObject.SetActive(false);
        }

        private TrailRenderer InitializeTrail(TrailRenderer trail)
        {
            trail.colorGradient = _trailConfiguration.Color;
            trail.material = _trailConfiguration.Material;
            trail.widthCurve = _trailConfiguration.WidthCurve;
            trail.time = _trailConfiguration.Duration;
            trail.minVertexDistance = _trailConfiguration.MinVertexDistance;

            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }
    }
}
