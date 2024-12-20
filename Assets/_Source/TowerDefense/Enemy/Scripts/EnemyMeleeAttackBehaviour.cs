using UnityEngine;

namespace EndlessRoad
{
    public class EnemyMeleeAttackBehaviour : StateMachineBehaviour
    {
        private EnemyAnimatorBase _enemyAnimator;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemyAnimator ??= animator.GetComponent<EnemyAnimatorBase>();
            _enemyAnimator.AttackEnded?.Invoke();
        }
    }
}
