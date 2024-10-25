using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace EndlessRoad.Shooter
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float smoothTime = 0.05f;
        [SerializeField] Weapon _weapon;
        [SerializeField] private Transform _camera;
        private bool _isShooting;

        private CharacterController _controller;
        private PlayerInput _playerInput;

        private Vector2 _inputRotation;
        private float _pitch;
        private Vector3 _prevValue;
        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private bool _isRotating;

        private ObjectPool _test;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _test = objectPool;
        }

        private void Awake()
        {
            _test.Initialize();
            _controller = GetComponent<CharacterController>();
            _playerInput = new();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Player.Jump.started += OnJump;

            _playerInput.Player.Rotation.started += ctx => { _inputRotation = ctx.ReadValue<Vector2>(); _isRotating = true; };
            _playerInput.Player.Rotation.canceled += ctx => _isRotating = false;

            _playerInput.Player.Fire.started += OnStartShoot;
            _playerInput.Player.Fire.canceled += ctx => _isShooting = false;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _playerInput.Player.Jump.started -= OnJump;
            _playerInput.Player.Rotation.started -= ctx => { _inputRotation = ctx.ReadValue<Vector2>(); _isRotating = true; };
            _playerInput.Player.Rotation.canceled -= ctx => _isRotating = false;

            _playerInput.Player.Fire.performed -= ctx => _weapon.Fire();
            _playerInput.Player.Fire.canceled -= ctx => _weapon.Fire();
        }

        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();
            Move(_playerInput.Player.Move.ReadValue<Vector2>());
        }

        private void LateUpdate()
        {
            if (_isRotating)
            {
                LookAtDirection(_inputRotation.normalized);
            }
        }

        private void Move(Vector2 input)
        {
            Vector3 move = _camera.transform.forward * input.y + _camera.transform.right * input.x;
            move.y = 0f;
            _controller.Move(move * _moveSpeed * Time.fixedDeltaTime);
            _playerVelocity.y += Physics.gravity.y * 2f * Time.fixedDeltaTime;

            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }

            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private bool IsGrounded() => _controller.isGrounded;

        private void LookAtDirection(Vector2 direction)
        {
            float mouseX = direction.x;
            float mouseY = direction.y;
            _prevValue = _camera.transform.localEulerAngles;
            _pitch -= mouseY * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, -80f, 80f);
            _camera.transform.eulerAngles = new Vector3(_pitch * 2f, _camera.transform.localEulerAngles.y, 0f);
            transform.Rotate(Vector3.up, mouseX * Time.deltaTime * 2f);
        }

        private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!_isGrounded) return;

            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3f * Physics.gravity.y);
        }


        private void OnStartShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            StartCoroutine(ShootRoutine());
        }

        private IEnumerator ShootRoutine()
        {
            _isShooting = true;

            while (_isShooting)
            {
                _weapon.Fire(); // выполняем выстрел
                yield return new WaitForSeconds(_weapon.FireRate); // ждём время между выстрелами
            }
        }

    }
}
