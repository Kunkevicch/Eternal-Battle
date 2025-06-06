using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class ImpactEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable() => StartCoroutine(ParticlePlayingRoutine());

        private IEnumerator ParticlePlayingRoutine()
        {
            while (_particleSystem.isPlaying)
            {
                yield return null;
            }

            ResetFX();
        }

        private void ResetFX() => gameObject.SetActive(false);

    }
}
