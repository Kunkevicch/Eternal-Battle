using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace EndlessRoad.Shooter
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _immortalityTimeAfterReviving;
        [SerializeField] private CinemachineCamera _camera;

        private WeaponHolder _weaponHolder;

        private bool _isAim;
        private bool _isShooting;
        private bool _isGrounded;
        private bool _isInjured;

        private CharacterController _controller;
        private Health _health;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;

        private ObjectPool _test; // TODO: Перенести обжект пул либо в gamecontroller, либо 
        private EventBus _eventBus;

        [Inject]
        public void Construct(ObjectPool objectPool, EventBus eventBus)
        {
            _test = objectPool;
            _eventBus = eventBus;
        }

        private Vector2 MoveDirection => _playerInput.Player.Move.ReadValue<Vector2>();
        private Vector2 lookDirection => _playerInput.Player.Rotation.ReadValue<Vector2>();
        private WeaponView ActiveWeapon => _weaponHolder.ActiveWeapon;

        private void Awake()
        {
            _test.Initialize();
            _controller = GetComponent<CharacterController>();
            _weaponHolder = GetComponentInChildren<WeaponHolder>();
            _health = GetComponent<Health>();
            _playerInput = new();
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.Move.started += ctx => ActiveWeapon.SetShootMove(true);
            _playerInput.Player.Move.canceled += ctx => ActiveWeapon.SetShootMove(false);

            _playerInput.Player.Jump.started += OnJump;

            _playerInput.Player.Fire.started += OnStartShooting;//_isShooting = true;
            _playerInput.Player.Fire.canceled += OnCancelShooting;//_isShooting = false;

            _playerInput.Player.Aim.started += OnAim;

            _playerInput.Player.ChangeWeapon.started += OnChangeWeapon;

            _playerInput.Player.Reload.started += ctx => _weaponHolder.Reload();

            _health.HealthChanged += OnHealthChanged;
            _health.Dead += OnDead;

            _eventBus.SecondChance += OnSecondChance;
        }

        private void OnDisable()
        {
            _playerInput.Disable();

            _playerInput.Player.Move.started -= ctx => _weaponHolder.ActiveWeapon.SetShootMove(true);
            _playerInput.Player.Move.canceled -= ctx => _weaponHolder.ActiveWeapon.SetShootMove(false);

            _playerInput.Player.Jump.started -= OnJump;

            _playerInput.Player.Fire.started -= OnStartShooting;
            _playerInput.Player.Fire.canceled -= OnCancelShooting;

            _playerInput.Player.Aim.started -= OnAim;

            _playerInput.Player.ChangeWeapon.started -= OnChangeWeapon;

            _playerInput.Player.Reload.started -= ctx => _weaponHolder.Reload();

            _health.HealthChanged -= OnHealthChanged;
            _health.Dead -= OnDead;

            _eventBus.SecondChance -= OnSecondChance;
        }

        private void Update()
        {
            _weaponHolder.transform.rotation = _camera.transform.rotation;
            _weaponHolder.Sway(lookDirection.x, lookDirection.y);
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

        private void OnStartShooting(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _isShooting = true;
            StartCoroutine(ShootingRoutine());
        }

        private IEnumerator ShootingRoutine()
        {
            while (_isShooting)
            {
                _weaponHolder.Shoot(_isShooting);
                yield return null;
            }
        }

        private void OnCancelShooting(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _isShooting = false;
            _weaponHolder.Shoot(false);
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

        private void OnDead()
        {
            _camera.Lens.FieldOfView = 60f;
            _playerInput.Player.Disable();
        }

        private void OnSecondChance()
        {
            _playerInput.Player.Enable();
            _health.Revive();
            _health.StartImmortality(_immortalityTimeAfterReviving);
        }

        // Игроку наносится урон, после которого у него остается <25% хп
        //  ->Выводить vfx получения критического урона
        //  ->При восстановлении хп выше 25% показывать VFX хила
        private void OnHealthChanged(int newHealth)
        {
            if ((float)_health.CurrentHealth / _health.MaxHealth < 0.25f)
            {
                _isInjured = true;
                _eventBus.RaisePlayerInjured();
            }
            else if (_isInjured)
            {
                _isInjured = false;
                _eventBus.RaisePlayerRestored();
            }
        }

    }
}
