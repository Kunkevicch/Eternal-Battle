using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "Gun_", menuName = "Configs/Weapon/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public WeaponType Type;
        public string Name;
        public WeaponView WeaponView;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        public ShootConfiguration ShootConfiguration;
        public TrailConfiguration TrailConfiguration;
        public int Damage;

        private MonoBehaviour _activeMonoBehaviour;
        private WeaponView _weaponView;
        private float _lastShootTime;
        private ParticleSystem _shootSystem;
        private ObjectPool _trailPool;

        private Vector3 ScreenCenter => new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        private Vector3 TargetPoint => Camera.main.ScreenToWorldPoint(new Vector3(ScreenCenter.x, ScreenCenter.y, Camera.main.farClipPlane));

        public WeaponView Spawn(Transform parent, MonoBehaviour activeMonoBehaviour, ObjectPool objectPool, ImpactService impactService)
        {
            _activeMonoBehaviour = activeMonoBehaviour;
            _trailPool = objectPool;
            _lastShootTime = 0;
            _weaponView = Instantiate(WeaponView.gameObject).GetComponent<WeaponView>();
            _weaponView.transform.SetParent(parent, false);
            _weaponView.transform.localPosition = SpawnPoint;
            _weaponView.transform.localRotation = Quaternion.Euler(SpawnRotation);

            _shootSystem = _weaponView.GetComponentInChildren<ParticleSystem>();

            _weaponView.Initialize(
                ShootConfiguration.ImpactMask
                , Damage
                , TrailConfiguration
                , ShootConfiguration
                , objectPool
                , impactService
                );

            return _weaponView;
        }

        public void Shoot()
        {
            if (Time.time > ShootConfiguration.FireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;
                _shootSystem.Play();
                Vector3 shootDirection = _shootSystem.transform.forward + new Vector3(Random.Range(-ShootConfiguration.Spread.x, ShootConfiguration.Spread.x), Random.Range(-ShootConfiguration.Spread.z, ShootConfiguration.Spread.z));
                //var shootDirection = GetSpreadPoint();

                shootDirection.Normalize();

                if (Physics.Raycast(
                    _shootSystem.transform.position
                    , shootDirection
                    , out RaycastHit hit
                    , float.MaxValue
                    , ShootConfiguration.ImpactMask
                    ))
                //if (Physics.Raycast(_shootSystem.transform.position, shootDirection, out RaycastHit hit, 1000f, ShootConfiguration.ImpactMask))
                {
                    _activeMonoBehaviour.StartCoroutine(PlayTrail(
                        _shootSystem.transform.position
                        , hit.point
                        , hit
                        ));
                }
                else
                {
                    _activeMonoBehaviour.StartCoroutine(
                        PlayTrail(
                        _shootSystem.transform.position
                        , _shootSystem.transform.position + (shootDirection * TrailConfiguration.MissDistance)
                        , new RaycastHit()
                        ));
                }
            }
        }

        private Vector3 GetSpreadPoint()
        {
            Vector3 randomSpread = Random.insideUnitCircle * 5f;
            Vector3 finalTarget = TargetPoint + randomSpread;
            return (finalTarget - _shootSystem.transform.position);
        }

        private TrailRenderer InitializeTrail(TrailRenderer trail)
        {
            trail.colorGradient = TrailConfiguration.Color;
            trail.material = TrailConfiguration.Material;
            trail.widthCurve = TrailConfiguration.WidthCurve;
            trail.time = TrailConfiguration.Duration;
            trail.minVertexDistance = TrailConfiguration.MinVertexDistance;

            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }

        private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
        {
            ShootTrail instance = (ShootTrail)_trailPool.ReuseComponent(TrailConfiguration.shootTrail.gameObject, startPoint, Quaternion.identity);
            Transform instanceParent = instance.transform.parent;
            instance.Trail.gameObject.SetActive(true);
            instance.transform.SetParent(_shootSystem.transform);
            InitializeTrail(instance.Trail);
            instance.transform.position = startPoint;
            //yield return null;
            instance.Trail.emitting = true;
            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    startPoint
                    , endPoint
                    , Mathf.Clamp01(1 - (remainingDistance / distance))
                    );
                remainingDistance -= TrailConfiguration.SimulationSpeed * Time.deltaTime;
                yield return null;
            }

            instance.transform.position = endPoint;

            yield return new WaitForSeconds(TrailConfiguration.Duration);

            yield return null;

            instance.Trail.emitting = false;
            instance.transform.SetParent(instanceParent);
            instance.gameObject.SetActive(false);
        }
    }
}
