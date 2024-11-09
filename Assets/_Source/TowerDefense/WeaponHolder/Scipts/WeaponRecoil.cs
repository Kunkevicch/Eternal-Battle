using UnityEngine;

namespace EndlessRoad
{
    public class WeaponRecoil : MonoBehaviour
    {
        [Header("Recoil settings")]
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _returnSpeed;

        [Header("Heapfire")]
        [SerializeField]
        private Vector3 _recoilRotation;

        [Header("Aiming")]
        [SerializeField]
        private Vector3 _recoilRotationAiming;

        private Transform _hand;

        private Vector3 _currentRotation;
        private Vector3 _rot;

        private bool _aiming;

        private void FixedUpdate()
        {
            if (!_hand)
                return;
            _currentRotation = Vector3.Lerp(_currentRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
            _rot = Vector3.Slerp(_rot, _currentRotation, _rotationSpeed * Time.fixedDeltaTime);
            _hand.transform.localRotation = Quaternion.Euler(_rot);
        }

        public void RecoilProcess()
        {
            if (_aiming)
            {
                _currentRotation += new Vector3(_recoilRotationAiming.x, Random.Range(-_recoilRotationAiming.y, _recoilRotationAiming.y), Random.Range(-_recoilRotationAiming.z, _recoilRotationAiming.z));
            }
            else
            {
                _currentRotation += new Vector3(_recoilRotation.x, Random.Range(-_recoilRotation.y, _recoilRotation.y), Random.Range(-_recoilRotation.z, _recoilRotation.z));
            }
        }

        public void SetAimState(bool state) => _aiming = state;
        
        public void SetHand(Transform hand) => _hand = hand;
    }
}
