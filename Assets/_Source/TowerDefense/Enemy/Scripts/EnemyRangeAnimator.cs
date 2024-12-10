using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace EndlessRoad
{
    public class EnemyRangeAnimator : EnemyAnimator
    {
        [Space(10)]
        [Header("Right Hand IK")]
        [SerializeField] private TwoBoneIKConstraint _rightHandIK;
        [SerializeField] private Transform _rightHandPositionDefaultIK;

        [Space(10)]
        [Header("Left Hand IK")]
        [SerializeField] private TwoBoneIKConstraint _leftHandIK;
        [SerializeField] private Transform _leftHandPositionDefaultIK;

        [Space(10)]
        [Header("Rig")]
        [SerializeField]
        private Rig _rig;
        private RigBuilder _rigBuilder;

        protected override void Awake()
        {
            base.Awake();
            _rigBuilder = GetComponent<RigBuilder>();
        }

        public override void Revive()
        {
            base.Revive();
            SetTargetPositionToWeaponIK();
        }

        public override void PlayIdle()
        {
            _weaponPosition.localPosition = _weaponIK.IdlePosition;
            _weaponPosition.localEulerAngles = _weaponIK.IdleRotation;
            base.PlayIdle();
        }

        public override void PlayMove()
        {
            base.PlayMove();
            _weaponPosition.localPosition = _weaponIK.FiringPosition;
            _weaponPosition.localEulerAngles = _weaponIK.FiringRotation;
        }

        public override void PlayAttack()
        {
            _weaponPosition.localPosition = _weaponIK.FiringPosition;
            _weaponPosition.localEulerAngles = _weaponIK.FiringRotation;
            base.PlayAttack();
        }

        public override void PlayDead()
        {
            _rig.weight = 0;
            base.PlayDead();
        }

        protected override void RefreshAnimator()
        {
            base.RefreshAnimator();
            _rigBuilder.Build();
        }

        private void SetTargetPositionToWeaponIK()
        {
            _rightHandIK.data.target = _weaponIK.RightHandPosition;
            _leftHandIK.data.target = _weaponIK.LeftHandPosition;
            _weaponIK.transform.SetParent(_weaponPosition);
            _weaponIK.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            _rig.weight = 1;
            RefreshAnimator();
        }
    }
}
