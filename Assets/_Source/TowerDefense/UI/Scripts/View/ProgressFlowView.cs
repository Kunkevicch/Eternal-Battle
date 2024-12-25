using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRoad
{
    public class ProgressFlowView : MonoBehaviour
    {
        [SerializeField] private Image _progressBorder;
        [SerializeField] private Image _progressFill;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private float _progressChangeTime;

        private Coroutine _changeRoutine;

        public void SetProgress(float newProgress)
        {
            StartChangeProgressRoutine(newProgress);
        }

        private void StartChangeProgressRoutine(float newProgress)
        {
            if (_changeRoutine != null)
            {
                StopCoroutine(_changeRoutine);
                _changeRoutine = null;
            }
            _changeRoutine = StartCoroutine(ChangeProgressRoutine(newProgress));
        }

        private IEnumerator ChangeProgressRoutine(float newProgress)
        {
            float startTime = Time.time;
            float startProgress = _progressFill.fillAmount;
            float endTime = startTime + _progressChangeTime;

            while (Time.time < endTime)
            {
                float elapsedTime = Time.time - startTime;
                float t = elapsedTime / _progressChangeTime;
                float updatedProgress = Mathf.Lerp(startProgress, newProgress, t);
                _progressFill.fillAmount = updatedProgress;
                int newProgressTextValue = Mathf.CeilToInt(updatedProgress * 100);
                _progressText.text = newProgressTextValue.ToString() + "%";
                yield return null;
            }

            _progressFill.fillAmount = newProgress;
            int finalProgressTextValue = Mathf.CeilToInt(newProgress * 100);
            _progressText.text = finalProgressTextValue.ToString() + "%";
        }
    }
}
