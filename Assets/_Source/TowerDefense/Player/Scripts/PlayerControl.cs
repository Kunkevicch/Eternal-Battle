using Cinemachine;
using System;
using UnityEngine;

namespace EndlessRoad.Shooter
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float smoothTime = 0.05f;

        private CharacterController _controller;
        private CinemachineVirtualCamera _camera;
        private PlayerInput _playerInput;

        private Vector3 _playerVelocity;
        private float xRotation;
        private bool _isGrounded;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<CinemachineVirtualCamera>();
            _playerInput = new();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Player.Jump.started += OnJump;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _playerInput.Player.Jump.started -= OnJump;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            Move(_playerInput.Player.Move.ReadValue<Vector2>());
        }

        private void LateUpdate()
        {
            LookAtDirection(_playerInput.Player.Rotation.ReadValue<Vector2>());
        }

        private void Move(Vector2 input)
        {
            Vector3 moveDirection = new(input.x, 0, input.y);
            _controller.Move(transform.TransformDirection(moveDirection) * _moveSpeed * Time.deltaTime);

            _playerVelocity.y += Physics.gravity.y * 2f * Time.deltaTime;

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
            xRotation -= (mouseY * Time.deltaTime) * 30f;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * 30f);
        }

        private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!_isGrounded) return;

            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3f * Physics.gravity.y);
        }
    }
}
