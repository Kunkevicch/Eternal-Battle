using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin _shaker;

        private WeaponHolder _weaponHolder;

        [Inject]
        public void Construct(WeaponHolder weaponHolder)
        {
            _weaponHolder = weaponHolder;
        }

        private void Awake()
        {
            _shaker = GetComponent<CinemachineBasicMultiChannelPerlin>();
            _shaker.enabled = false;
        }

        private void OnEnable()
        {
            _weaponHolder.WeaponShooted += OnWeaponShooted;
        }

        private void OnDisable()
        {
            _weaponHolder.WeaponShooted -= OnWeaponShooted;
        }

        private void OnWeaponShooted(bool isShooted) => _shaker.enabled = isShooted;
    }
}
