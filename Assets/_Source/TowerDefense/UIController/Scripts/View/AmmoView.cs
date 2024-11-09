using TMPro;
using UnityEngine;

namespace EndlessRoad
{
    public sealed class AmmoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammoText;

        public void AmmoChange(string newAmmo) => _ammoText.text = newAmmo;
    }
}
