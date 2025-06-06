using TMPro;
using UnityEngine;

namespace EndlessRoad
{
    public class TestFPS : MonoBehaviour
    {
        private TextMeshProUGUI _textFPS;
        private void Awake()
        {
            _textFPS = GetComponent<TextMeshProUGUI>();
            InvokeRepeating(nameof(test),1,1);
        }

        // Update is called once per frame
        private void test()
        {
            float fps = (int)1.0f / Time.unscaledDeltaTime;
            _textFPS.text = fps.ToString();
        }
    }
}
