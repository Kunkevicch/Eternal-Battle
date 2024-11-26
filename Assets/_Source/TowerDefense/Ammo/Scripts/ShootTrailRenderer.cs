using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class ShootTrailRenderer : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            StartCoroutine(ResetTrailRendererRoutine());
        }

        private void OnDisable()
        {
            _trailRenderer.emitting = false;
            _trailRenderer.enabled = false;
        }

        private IEnumerator ResetTrailRendererRoutine()
        {
            yield return new WaitForEndOfFrame();
            _trailRenderer.emitting = true;
            _trailRenderer.enabled = true;
        }
    }
}
