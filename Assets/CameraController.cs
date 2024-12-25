using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin _shaker;
        private CinemachineCamera _camera;
        private WeaponHolder _weaponHolder;
        private EventBus _eventBus;

        [Inject]
        public void Construct(WeaponHolder weaponHolder, EventBus eventBus)
        {
            _weaponHolder = weaponHolder;
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
            _shaker = GetComponent<CinemachineBasicMultiChannelPerlin>();
            _shaker.enabled = false;
        }

        private void OnEnable()
        {
            _weaponHolder.WeaponShooted += OnWeaponShooted;
            _eventBus.GameOver -= OnGameOver;
            _eventBus.SecondChance -= OnGameRestarted;
        }

        private void OnDisable()
        {
            _weaponHolder.WeaponShooted -= OnWeaponShooted;
            _eventBus.GameOver -= OnGameOver;
            _eventBus.SecondChance -= OnGameRestarted;
        }

        private void OnWeaponShooted(bool isShooted) => _shaker.enabled = isShooted;

        private void OnGameOver()
        {
            _camera.enabled = false;
        }

        private void OnGameRestarted()
        {
            _camera.enabled = true;
        }
    }
}
