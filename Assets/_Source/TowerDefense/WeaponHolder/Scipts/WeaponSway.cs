using UnityEngine;

namespace EndlessRoad
{
    public class WeaponSway : MonoBehaviour
    {
        [SerializeField] private float _amount;
        [SerializeField] private float _maxAmount;
        [SerializeField] private float _smoothAmount;

        [SerializeField] private float _rotationAmount;
        [SerializeField] private float _maxRotationAmount;
        [SerializeField] private float _smoothRotation;

        [SerializeField] private bool _rotationX;
        [SerializeField] private bool _rotationY;
        [SerializeField] private bool _rotationZ;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private float _inputX;
        private float _inputY;

        private WeaponHolder _weaponHolder;

        private void Awake()
        {
            _weaponHolder = GetComponent<WeaponHolder>();
        }

        public void SetInitialPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _initialPosition = position;
            _initialRotation = rotation;
        }

        public void SwayProcess(float X, float Y)
        {
            SetSway(X, Y);
            MoveSway();
            TiltSway();
        }

        private void SetSway(float X, float Y)
        {
            _inputX = X;
            _inputY = Y;
        }

        private void MoveSway()
        {
            float moveX = Mathf.Clamp(_inputX * _amount, -_maxAmount, _maxAmount);
            float moveY = Mathf.Clamp(_inputY * _amount, -_maxAmount, _maxAmount);

            Vector3 finalPosition = new Vector3(-moveX, -moveY, 0);

            _weaponHolder.Hand.transform.localPosition = Vector3.Lerp(_weaponHolder.Hand.transform.localPosition, finalPosition + _initialPosition, Time.deltaTime * _smoothAmount);
        }

        private void TiltSway()
        {
            float tiltY = Mathf.Clamp(_inputX * _rotationAmount, -_maxRotationAmount, _maxRotationAmount);
            float tiltX = Mathf.Clamp(_inputY * _rotationAmount, -_maxRotationAmount, _maxRotationAmount);

            Quaternion finalRotation = Quaternion.Euler(new Vector3(
                _rotationX ? -tiltX : 0,
                _rotationY ? tiltY : 0,
                _rotationZ ? -tiltY : 0
                ));

            _weaponHolder.Hand.transform.localRotation = Quaternion.Slerp(_weaponHolder.Hand.transform.localRotation, finalRotation * _initialRotation, Time.deltaTime * _smoothAmount);
        }
    }
}
