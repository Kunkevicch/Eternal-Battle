using UnityEngine;

namespace EndlessRoad
{
    public class ImpactEffect : MonoBehaviour
    {
        private void OnEnable() => Invoke(nameof(ResetFX), 0.4f);
        private void ResetFX() => gameObject.SetActive(false);
    }
}
