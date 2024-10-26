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
        [SerializeField] WeaponConfig _weapon;
        [SerializeField] private Transform _camera;
        private bool _isShooting;


        private CharacterController _controller;
        private PlayerInput _playerInput;
        private WeaponView _weaponView;
        private Vector2 _inputRotation;
        private float _pitch;
        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private bool _isRotating;

        private ObjectPool _test;
        private ImpactService _impactService;

        [Inject]
        private void Construct(ObjectPool objectPool, ImpactService impactService)
        {
            _test = objectPool;
            _impactService = impactService;
        }

        private Vector2 MoveDirection => _playerInput.Player.Move.ReadValue<Vector2>();

        private void Awake()
        {
            _test.Initialize();
            _controller = GetComponent<CharacterController>();
            _playerInput = new();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _weaponView = _weapon.Spawn(_camera.transform, this, _test, _impactService);
        }

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.Move.started += ctx => _weaponView.SetShootMove(true);
            _playerInput.Player.Move.canceled += ctx => _weaponView.SetShootMove(false);

            _playerInput.Player.Jump.started += OnJump;

            _playerInput.Player.Rotation.started += ctx => { _inputRotation = ctx.ReadValue<Vector2>(); _isRotating = true; };
            _playerInput.Player.Rotation.canceled += ctx => _isRotating = false;

            _playerInput.Player.Fire.started += ctx => _isShooting = true;
            _playerInput.Player.Fire.canceled += ctx => _isShooting = false;
        }

        private void OnDisable()
        {
            _playerInput.Disable();

            _playerInput.Player.Move.started -= ctx => _weaponView.SetShootMove(true);
            _playerInput.Player.Move.canceled -= ctx => _weaponView.SetShootMove(false);

            _playerInput.Player.Jump.started -= OnJump;
            _playerInput.Player.Rotation.started -= ctx => { _inputRotation = ctx.ReadValue<Vector2>(); _isRotating = true; };
            _playerInput.Player.Rotation.canceled -= ctx => _isRotating = false;

            _playerInput.Player.Fire.started -= ctx => _isShooting = true;
            _playerInput.Player.Fire.canceled -= ctx => _isShooting = false;
        }


        private void Update()
        {
            if (_isShooting)
            {
                _weaponView.Shoot();
            }
        }

        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();
            Move(MoveDirection);
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
                _weapon.Shoot(); // выполняем выстрел
                yield return null;// ждём время между выстрелами
            }
        }

    }
}
