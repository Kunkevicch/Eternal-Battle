using System.Collections;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShootTrail : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        [Inject]
        private ImpactService _impactService;

        public TrailRenderer Trail => _trailRenderer;

        public void StartLaunch(Vector3 startPoint, Vector3 endPoint, RaycastHit hit, float speed, float duration, int damage)
        {
            StartCoroutine(LaunchTrailRoutine(startPoint, endPoint, hit, speed, duration, damage));
        }

        private IEnumerator LaunchTrailRoutine(Vector3 startPoint, Vector3 endPoint, RaycastHit hit, float speed, float duration, int damage)
        {
            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;

            bool isActive = false;

            while (remainingDistance > 0)
            {
                transform.position = Vector3.Lerp(
                    startPoint
                    , endPoint
                    , Mathf.Clamp01(1 - (remainingDistance / distance))
                    );
                remainingDistance -= speed * Time.deltaTime;

                yield return null;

                if (!isActive)
                {
                    isActive = true;
                    _trailRenderer.emitting = true;
                    _trailRenderer.gameObject.SetActive(true);
                }
            }

            transform.position = endPoint;

            yield return new WaitForSeconds(duration);

            yield return null;

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IImpactable impactable))
                {
                    _impactService.DoImpactToTargetPoint(impactable, hit.point);
                    if (impactable is IDamageable damageable)
                    {
                        damageable.ApplyDamage(damage);
                    }
                }
            }

            _trailRenderer.emitting = false;
            _trailRenderer.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
