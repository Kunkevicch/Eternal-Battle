using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class CrossHairView : MonoBehaviour
    {
        [SerializeField] private float _maxSize;
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _minSize;

        private float _currentSize;
        private float _currentHeight;

        private float _resizeTime;
        private float _heightTime;

        private RectTransform _transform;

        private Coroutine _resizeCoroutine;
        private Coroutine _resetHeightCoroutine;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        public void UpdateSize(float spreadRadius)
        {
            CancelSizeCoroutine();
            _resizeTime += Time.deltaTime;
            _currentSize = Mathf.Lerp(_currentSize, _maxSize, Mathf.Clamp01(_resizeTime));
            _transform.sizeDelta = new Vector2(_currentSize, _currentSize);
        }

        public void UpdateHeight(float height)
        {
            CancelHeightCoroutine();
            _currentHeight = Mathf.Clamp(_currentHeight + height, 0, _maxHeight);
            _transform.localPosition = new Vector3(0, _currentHeight, 0);
        }

        public void StartResetHeight()
        {
            CancelHeightCoroutine();
            _resetHeightCoroutine = StartCoroutine(ResetHeightRoutine());
        }

        private IEnumerator ResetHeightRoutine()
        {
            float startTime = Time.time;
            float t;
            while (_currentHeight > 0)
            {
                t = (Time.time - startTime) / 0.2f;
                _currentHeight = Mathf.Lerp(_currentHeight, 0, t);
                _transform.localPosition = new Vector3(0, _currentHeight, 0);
                yield return null;
            }
            _transform.localPosition = Vector3.zero;
        }

        public void StartResetSize()
        {
            CancelSizeCoroutine();
            _resizeCoroutine = StartCoroutine(ResetSizeRoutine());
        }

        private IEnumerator ResetSizeRoutine()
        {
            _resizeTime = 0;

            float startTime = Time.time;
            float t;

            while (_currentSize > _minSize)
            {
                t = (Time.time - startTime) / 0.2f;
                _currentSize = Mathf.Lerp(_currentSize, _minSize, t);
                _transform.sizeDelta = new Vector2(_currentSize, _currentSize);
                yield return null;
            }
            _currentSize = _minSize;
            _transform.sizeDelta = new Vector2(_currentSize, _currentSize);
        }

        private void CancelSizeCoroutine()
        {
            if (_resizeCoroutine != null)
            {
                StopCoroutine(_resizeCoroutine);
                _resizeCoroutine = null;
            }
        }

        private void CancelHeightCoroutine()
        {
            _heightTime = 0;
            if (_resetHeightCoroutine != null)
            {
                StopCoroutine(_resetHeightCoroutine);
                _resetHeightCoroutine = null;
            }
        }

    }
}
