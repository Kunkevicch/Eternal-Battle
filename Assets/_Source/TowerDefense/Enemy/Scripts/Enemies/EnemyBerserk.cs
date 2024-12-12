using System;
using UnityEngine;

namespace EndlessRoad
{
    public class EnemyBerserk : EnemyBase
    {
        [SerializeField] private float _rageDuration;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rageMoveSpeed;

        private bool _canAttack = true;
        private float _moveSpeedTemp;
        private BerserkRage _berserkRage;

        private Action _rageCallback;
        private Action _rageOverCallback;

        protected override void Awake()
        {
            base.Awake();
            _berserkRage = new BerserkRage(_rageDuration);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _animator.AttackEnded += OnAttackEnded;

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _animator.AttackEnded -= OnAttackEnded;
        }
        public override void Initialize()
        {
            _combat.Initialize(_weaponConfig);
            _animator.Initalize();
            _agent.enabled = true;
        }

        public override void Revive()
        {
            _berserkRage.Revive();
            _canAttack = true;
            _health.SetMinimalHealth(1);
            base.Revive();
        }

        public float MoveSpeed => _moveSpeed;
        public bool CanRage => (float)_health.CurrentHealth / _health.MaxHealth <= 0.5f && _berserkRage.CanRage;
        public bool CanAtack => _canAttack;

        public override void CombatProcess()
        {
            _canAttack = false;
            _animator.PlayAttack();
        }

        public void StartRageProcess(Action rageCompleteCallback, Action rageOverCallback)
        {
            _rageCallback = rageCompleteCallback;
            _rageOverCallback = rageOverCallback;
            _animator.PlayClipWithName("Rage");
        }
        public void RageComplete()
        {
            _moveSpeedTemp = _moveSpeed;
            _moveSpeed = _rageMoveSpeed;

            _rageCallback();
            _animator.SetFloatParamValue("AnimationSpeed", 2);
            StartCoroutine(_berserkRage.RageRoutine(() => OnRageEnded()));
        }

        private void OnAttackEnded() => _canAttack = true;

        private void OnRageEnded()
        {
            _health.SetMinimalHealth(0);
            _moveSpeed = _moveSpeedTemp;
            _animator.SetFloatParamValue("AnimationSpeed", 1);
            _rageOverCallback();
        }

        public override void DeadProcess()
        {
            if (_berserkRage.IsBerserk || _berserkRage.CanRage)
                return;
            base.DeadProcess();
        }
    }
}