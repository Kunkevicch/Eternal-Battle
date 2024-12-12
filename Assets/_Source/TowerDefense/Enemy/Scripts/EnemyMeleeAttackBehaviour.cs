using UnityEngine;

namespace EndlessRoad
{
    public class EnemyMeleeAttackBehaviour : StateMachineBehaviour
    {
        private EnemyAnimator _enemyAnimator;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemyAnimator ??= animator.GetComponent<EnemyAnimator>();
            _enemyAnimator.AttackEnded?.Invoke();
        }
    }
}
