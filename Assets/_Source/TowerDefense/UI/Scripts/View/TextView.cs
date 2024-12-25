using TMPro;
using UnityEngine;

namespace EndlessRoad
{
    public sealed class TextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void TextChange(string newText) => _text.text = newText;
    }
}
