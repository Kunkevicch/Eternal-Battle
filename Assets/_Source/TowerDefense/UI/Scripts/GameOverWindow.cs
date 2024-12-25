using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EndlessRoad
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _livesCountText;
        [SerializeField] private GameObject _restartGameBtnGO;
        [SerializeField] private Image _backgroundImage;

        private Button _restartGameBtn;
        private GameObserver _gameObserver;
        private EventBus _bus;

        [Inject]
        public void Construct(EventBus bus, GameObserver gameObserver)
        {
            _bus = bus;
            _gameObserver = gameObserver;
        }

        private void Awake()
        {
            _restartGameBtn = _restartGameBtnGO.GetComponent<Button>();
            Debug.Log(_gameObserver.LivesCount);
        }

        private void OnEnable()
        {
            StartCoroutine(FadeBackgroundRoutine());
            _livesCountText.text = _gameObserver.LivesCount.ToString();
            if (_gameObserver.LivesCount == 0)
            {
                _restartGameBtn.interactable = false;
            }
            else
            {
                _restartGameBtn.interactable = true;
            }
        }

        public void SecondChance()
        {
            _bus.RaiseSecondChance();
            gameObject.SetActive(false);
        }

        public IEnumerator FadeBackgroundRoutine()
        {
            float startTime = Time.time;
            float endTime = startTime + 5f;

            while (Time.time < endTime)
            {
                float elapsedTime = Time.time - startTime;
                float t = elapsedTime / endTime;
                float a = Mathf.Lerp(0, 255, t);
                _backgroundImage.color = new Color(0, 0, 0, a);
                yield return null;
            }
        }
    }
}
