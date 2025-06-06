using UnityEngine;

namespace EndlessRoad
{
    public class EnemyAnimatorMelee : EnemyAnimatorBase
    {
        [SerializeField] private int _attackCount;

        public override void Revive()
        {
            base.Revive();
            _weaponIK.transform.SetParent(_weaponPosition);
            _weaponIK.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }

        public override void PlayAttack() => _animator.Play($"Attack{Random.Range(1, _attackCount)}");
    }
}