using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace EndlessRoad
{
    public class ProgressFlowView : MonoBehaviour
    {
        [SerializeField] private Image _progressBorder;
        [SerializeField] private Image _progressFill;
        [SerializeField] private float _progressChangeTime;

        Tween _animation;

        public void SetProgress(float newProgress)
        {
            _animation = _progressFill.DOFillAmount(newProgress * _progressBorder.fillAmount, _progressChangeTime).Play();
        }

        public void SetProgress(float newProgress, float changeTime)
        {
            _animation = _progressFill.DOFillAmount(newProgress, changeTime).Play();
        }

        public void ResetProgress()
        {
            if (!_animation.IsComplete())
            {
                _animation.Complete();
            }
            _progressFill.DOFillAmount(1, 0).Play();
        }
    }
}
