using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverWindows;

        private EventBus _bus;

        [Inject]
        public void Construct(EventBus bus)
        {
            _bus = bus;
        }

        private void OnEnable()
        {
            _bus.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            _bus.GameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            _gameOverWindows.SetActive(true);
        }
    }
}
