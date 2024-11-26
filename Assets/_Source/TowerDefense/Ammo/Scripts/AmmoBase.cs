using System.Collections;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public abstract class AmmoBase : MonoBehaviour, IFireable
    {
        [Inject]
        protected ImpactService _impactService;

        protected int _damage;

        public void ActivateAmmo() => gameObject.SetActive(true);

        public void StartLaunch(Vector3 startPoint, Vector3 endPoint, RaycastHit hit, float speed, float duration, int damage)
        {
            StartCoroutine(LaunchAmmo(startPoint, endPoint, hit, speed, duration, damage));
        }

        protected IEnumerator LaunchAmmo(Vector3 startPoint, Vector3 endPoint, RaycastHit hit, float speed, float duration, int damage)
        {
            _damage = damage;
            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;

            while (remainingDistance > 0)
            {
                transform.position = Vector3.Lerp(
                    startPoint
                    , endPoint
                    , Mathf.Clamp01(1 - (remainingDistance / distance))
                    );
                remainingDistance -= speed * Time.deltaTime;

                yield return null;

            }

            transform.position = endPoint;

            yield return new WaitForSeconds(duration);

            yield return null;

            DoImpactToPoint(hit);

            gameObject.SetActive(false);
        }

        protected abstract void DoImpactToPoint(RaycastHit hit);
    }
}
