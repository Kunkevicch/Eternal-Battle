using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace EndlessRoad.Shooter
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private PlayerWeaponHolderConfig _weaponHolderConfig;

        private WeaponHolder _weaponHolder;

        private bool _isAim;
        private bool _isShooting;
        private bool _isGrounded;

        private CharacterController _controller;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;

        private ObjectPool _test; // TODO: Перенести обжект пул либо в gamecontroller, либо 

        [Inject]
        public void Construct(ObjectPool objectPool)
        {
            _test = objectPool;
        }

        private Vector2 MoveDirection => _playerInput.Player.Move.ReadValue<Vector2>();
        private Vector2 lookDirection => _playerInput.Player.Rotation.ReadValue<Vector2>();
        private WeaponView ActiveWeapon => _weaponHolder.ActiveWeapon;

        private void Awake()
        {
            _test.Initialize();
            _controller = GetComponent<CharacterController>();
            _weaponHolder = _camera.GetComponentInChildren<WeaponHolder>();
            _playerInput = new();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            _weaponHolderConfig.SpawnWeaponInHolder(_weaponHolder, _test);
        }

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.Move.started += ctx => ActiveWeapon.SetShootMove(true);
            _playerInput.Player.Move.canceled += ctx => ActiveWeapon.SetShootMove(false);

            _playerInput.Player.Jump.started += OnJump;

            _playerInput.Player.Fire.started += ctx => _isShooting = true;
            _playerInput.Player.Fire.canceled += ctx => _isShooting = false;

            _playerInput.Player.Aim.started += OnAim;

            _playerInput.Player.ChangeWeapon.started += OnChangeWeapon;

            _playerInput.Player.Reload.started += ctx => ActiveWeapon.StartReload();
        }

        private void OnDisable()
        {
            _playerInput.Disable();

            _playerInput.Player.Move.started -= ctx => _weaponHolder.ActiveWeapon.SetShootMove(true);
            _playerInput.Player.Move.canceled -= ctx => _weaponHolder.ActiveWeapon.SetShootMove(false);

            _playerInput.Player.Jump.started -= OnJump;

            _playerInput.Player.Fire.started -= ctx => _isShooting = true;
            _playerInput.Player.Fire.canceled -= ctx => _isShooting = false;

            _playerInput.Player.Aim.started -= OnAim;

            _playerInput.Player.ChangeWeapon.started -= OnChangeWeapon;

            _playerInput.Player.Reload.started -= ctx => ActiveWeapon.StartReload();
        }

        private void Update()
        {
            _weaponHolder.Sway(lookDirection.x, lookDirection.y);
            _weaponHolder.Shoot(_isShooting);
        }

        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();
            Move(MoveDirection);
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

            _controller.Move(_playerVelocity * Time.fixedDeltaTime);
        }

        private bool IsGrounded() => _controller.isGrounded;

        private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!_isGrounded) return;

            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3f * Physics.gravity.y);
        }

        private void OnChangeWeapon(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (_isAim || _isShooting)
                return;

            _weaponHolder.SetWeapon(context.ReadValue<float>() < 0 ? SetWeaponDirection.Previous : SetWeaponDirection.Next);
        }

        private void OnAim(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _isAim = !_isAim;
            if (_isAim)
            {
                _camera.Lens.FieldOfView = 30f;
            }
            else
            {
                _camera.Lens.FieldOfView = 60f;
            }
            StopCoroutine(AimRoutine());
            StartCoroutine(AimRoutine());
            _weaponHolder.Aim(_isAim);
        }

        private IEnumerator AimRoutine()
        {
            float startTime = Time.time;
            float t = 0;
            if (_isAim)
            {
                while (t < 1f)
                {
                    t = (Time.time - startTime) / 0.2f;
                    _camera.Lens.FieldOfView = Mathf.Lerp(60f, 30f, t);
                    yield return null;
                }
            }
            else
            {
                while (t < 1f)
                {
                    t = (Time.time - startTime) / 0.2f;
                    _camera.Lens.FieldOfView = Mathf.Lerp(30f, 60f, t);
                    yield return null;
                }
            }
        }
    }
}
