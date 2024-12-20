using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class EnemyMutantSimple : EnemyBase
    {
        public override void Initialize()
        {
            _combat.Initialize(_weaponConfig);
            _animator.Initalize();
            _agent.enabled = true;
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

        private void OnAttackEnded() => _canAttack = true;

        public override void CombatProcess()
        {
            _canAttack = false;
            _animator.PlayAttack();
        }
    }
}
