using UnityEngine;

namespace EndlessRoad
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class WeaponViewIK : MonoBehaviour
    {
        [SerializeField] private Transform _rightHandPosition;
        [SerializeField] private Transform _leftHandPosition;

        [SerializeField] private Vector3 _idlePosition;
        [SerializeField] private Vector3 _idleRotation;

        [SerializeField] private Vector3 _firingPosition;
        [SerializeField] private Vector3 _firingRotation;

        private Rigidbody _rigidBody;
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
            DisablePhysicBehaviour();
        }

        public Transform RightHandPosition => _rightHandPosition;
        public Transform LeftHandPosition => _leftHandPosition;

        public Vector3 IdlePosition => _idlePosition;
        public Vector3 IdleRotation => _idleRotation;

        public Vector3 FiringPosition => _firingPosition;
        public Vector3 FiringRotation => _firingRotation;

        public void EnablePhysicBehaviour()
        {
            _rigidBody.isKinematic = false;
            _boxCollider.enabled = true;
        }

        public void DisablePhysicBehaviour()
        {
            _rigidBody.isKinematic = true;
            _boxCollider.enabled = false;
        }
    }
}
