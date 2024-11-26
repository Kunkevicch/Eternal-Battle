using System.Collections;
using System.Linq;
using UnityEngine;

namespace EndlessRoad
{
    public class AmmoExplosion : MonoBehaviour, IExplosion
    {
        [SerializeField] private ParticleSystem _explosionFX;

        //private CinemachineImpulseSource _impulseSource;

        private int _damage;
        private float _damageRadius;
        private int _maxAffectedEnemies;
        private Collider[] _hits;
        private LayerMask _damageMask;

        private void Awake()
        {
            //_impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void InitializeExplosion(
            int damage
            , float damageRadius
            , int maxAffectedEnemies
            , LayerMask damageMask
            )
        {
            _damage = damage;
            _damageRadius = damageRadius;
            _maxAffectedEnemies = maxAffectedEnemies;
            _damageMask = damageMask;
            SetExplosionSize();
        }

        public void ActivateExplosion()
        {
            gameObject.SetActive(true);
            StartCoroutine(ApplyDamageRoutine());
        }

        private void SetExplosionSize()
        {
            var mainModule = _explosionFX.main;
            mainModule.startSize = _damageRadius * 2;
            mainModule.startSpeed = _damageRadius;
        }

        private IEnumerator ApplyDamageRoutine()
        {
            _hits = new Collider[_maxAffectedEnemies];

            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _damageRadius, _hits, _damageMask);
            var hittedEnemies = _hits.Where(x => x != null).ToList();
            _explosionFX.gameObject.SetActive(true);
            //_impulseSource.GenerateImpulse();
            while (hittedEnemies.Count > 0)
            {
                var hittedEnemy = hittedEnemies.Last();
                if (hittedEnemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(_damage);
                }
                hittedEnemies.Remove(hittedEnemy);
                yield return null;
            }

            while (_explosionFX.isPlaying)
            {
                yield return null;
            }

            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _damageRadius);
        }
    }
}
