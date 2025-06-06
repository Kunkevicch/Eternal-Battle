using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;
using System.Collections;

namespace EndlessRoad
{
    public class HitEffect : MonoBehaviour
    {
        private Vignette _vignette;

        private Coroutine _injuredCoroutine;
        private Coroutine _restoredCoroutine;
        private Coroutine _hittedRoutine;

        private EventBus _eventBus;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            if (GetComponent<Volume>().profile.TryGet(out Vignette vignette))
            {
                _vignette = vignette;
            }
        }

        private void OnEnable()
        {
            _eventBus.PlayerInjured += OnPlayerInjured;
            _eventBus.PlayerRestored += OnPlayerRestored;
            _eventBus.PlayerHitted += OnPlayerHitted;
        }

        private void OnDisable()
        {
            _eventBus.PlayerInjured -= OnPlayerInjured;
            _eventBus.PlayerRestored -= OnPlayerRestored;
            _eventBus.PlayerHitted -= OnPlayerHitted;
        }

        private void OnPlayerInjured()
        {
            if (_restoredCoroutine != null)
            {
                StopCoroutine(_restoredCoroutine);
                _restoredCoroutine = null;
            }

            StartCoroutine(ref _injuredCoroutine, PlayerInjuredRoutine());
        }

        private IEnumerator PlayerInjuredRoutine()
        {
            while (true)
            {
                float startTime = Time.time;
                float endTime = startTime + 0.5f;

                while (Time.time < endTime)
                {
                    float t = (Time.time - startTime) / 0.5f;
                    _vignette.intensity.Override(Mathf.Lerp(0.25f, 0.4f, t));
                    yield return null;
                }

                startTime = Time.time;
                endTime = startTime + 2f;

                while (Time.time < endTime)
                {
                    float t = (Time.time - startTime) / 0.5f;
                    _vignette.intensity.Override(Mathf.Lerp(0.4f, 0.25f, t));
                    yield return null;
                }
            }
        }

        private void OnPlayerRestored()
        {
            if (_injuredCoroutine != null)
            {
                StopCoroutine(_injuredCoroutine);
                _injuredCoroutine = null;
            }

            if (_hittedRoutine != null)
            {
                StopCoroutine(_hittedRoutine);
                _hittedRoutine = null;
            }

            StartCoroutine(ref _restoredCoroutine, PlayerRestoredRoutine());
        }

        private IEnumerator PlayerRestoredRoutine()
        {
            float startTime = Time.time;
            float endTime = startTime + 2f;
            float startValue = _vignette.intensity.value;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / 2f;
                _vignette.intensity.Override(Mathf.Lerp(startValue, 0, t));
                yield return null;
            }
            _vignette.intensity.Override(0);
        }

        private void OnPlayerHitted()
        {
            if (_injuredCoroutine != null)
                return;

            if (_restoredCoroutine != null)
            {
                StopCoroutine(_restoredCoroutine);
                _restoredCoroutine = null;
            }

            StartCoroutine(ref _hittedRoutine, PlayHittedRoutine());
        }

        private IEnumerator PlayHittedRoutine()
        {
            float startTime = Time.time;
            float endTime = startTime + 0.2f;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / 0.2f;
                _vignette.intensity.Override(Mathf.Lerp(0, 0.26f, t));
                yield return null;
            }
            _vignette.intensity.Override(0);
        }

        private void StartCoroutine(ref Coroutine coroutine, IEnumerator routine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                _injuredCoroutine = null;
            }
            coroutine = StartCoroutine(routine);
        }
    }
}
