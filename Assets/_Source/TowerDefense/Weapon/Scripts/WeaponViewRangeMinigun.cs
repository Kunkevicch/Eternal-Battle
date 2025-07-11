using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class WeaponViewRangeMinigun : WeaponViewRange
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _spinUpTime = 2f;
        [SerializeField] private float _maxRotationSpeed = 360f;
        [SerializeField] private float _spinDownTime = 2f;

        private float _currentRotationSpeed = 0f;
        private float _currentRotationAngle = 0f;
        private float _firingTime = 0f;
        private bool _isFiring;
        private bool _isSpinningUp;

        private Coroutine _rotateRoutine;
        private Coroutine _stopRotateRoutine;

        private void OnDisable()
        {
            _firingTime = 0f;
            _isFiring = false;
            _isSpinningUp = false;
            _currentRotationSpeed = 0f;
        }

        public override void Tick(bool wantsToAttack)
        {
            base.Tick(wantsToAttack);
            TickProcess(wantsToAttack);
        }

        public override void Tick(bool wantsToAttack, out bool canAttack)
        {
            base.Tick(wantsToAttack, out canAttack);
            TickProcess(wantsToAttack);
        }

        public override void StartReload()
        {
            _firingTime = 0;
            _isFiring = false;

            if (_rotateRoutine != null)
            {
                StopCoroutine(_rotateRoutine);
            }

            if (_stopRotateRoutine != null)
            {
                StopCoroutine(_stopRotateRoutine);
            }
            base.StartReload();
        }

        private void TickProcess(bool wantsToAttack)
        {
            if (wantsToAttack && !_isFiring)
            {
                StartFiring();
            }
            else if (!wantsToAttack && _isFiring)
            {
                StopFiring();
            }
        }

        protected override void Attack()
        {
            if (_firingTime >= _spinUpTime)
            {
                base.Attack();
            }
        }

        private void StartFiring()
        {
            _isFiring = true;
            if (!_isSpinningUp)
            {
                _firingTime = 0f;
                _currentRotationSpeed = 0f;
                _currentRotationAngle = 0f;
                _isSpinningUp = true;
            }

            if (_rotateRoutine != null)
            {
                StopCoroutine(_rotateRoutine);
            }

            if (_stopRotateRoutine != null)
            {
                StopCoroutine(_stopRotateRoutine);
            }

            _rotateRoutine = StartCoroutine(RotateBarrelRoutine());
        }

        private void StopFiring()
        {
            _isFiring = false;

            if (_rotateRoutine != null)
            {
                StopCoroutine(_rotateRoutine);
            }

            if (_stopRotateRoutine != null)
            {
                StopCoroutine(_stopRotateRoutine);
            }

            _stopRotateRoutine = StartCoroutine(StopBarrelRotateRoutine());
        }

        private IEnumerator RotateBarrelRoutine()
        {
            while (_isFiring)
            {
                _firingTime = Mathf.Clamp(_firingTime + Time.deltaTime, 0, _spinUpTime);
                _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, _maxRotationSpeed, (_firingTime / _spinUpTime));
                _currentRotationAngle += _currentRotationSpeed * Time.deltaTime;

                if (_currentRotationAngle >= 360f)
                {
                    _currentRotationAngle -= 360f;
                }

                _barrel.localEulerAngles = new Vector3(0, 0, _currentRotationAngle);

                yield return null;
            }
        }

        private IEnumerator StopBarrelRotateRoutine()
        {
            float spinDownTime = _spinDownTime;
            while (spinDownTime > 0)
            {
                _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, 0, 1 - (spinDownTime / _spinDownTime));

                _currentRotationAngle += _currentRotationSpeed * Time.deltaTime;

                if (_currentRotationAngle >= 360f)
                {
                    _currentRotationAngle -= 360f;
                }

                _barrel.localEulerAngles = new Vector3(0, 0, _currentRotationAngle);
                spinDownTime -= Time.deltaTime;
                _firingTime -= Time.deltaTime;
                yield return null;
            }

            _currentRotationSpeed = 0f;
            _isSpinningUp = false;
            _firingTime = 0;
        }
    }
}
