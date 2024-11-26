using UnityEngine;

namespace EndlessRoad
{
    public class ImpactableObject : MonoBehaviour, IImpactable
    {
        [field: SerializeField] public ImpactEffect ImpactPrefab { get; set; }
    }
}