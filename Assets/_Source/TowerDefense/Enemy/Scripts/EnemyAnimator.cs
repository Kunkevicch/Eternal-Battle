using UnityEngine;

namespace EndlessRoad
{
    public abstract class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] protected Transform _weaponPosition;

        protected Animator _animator;
        protected WeaponViewIK _weaponIK;
        protected RagdollHelper _ragdollHelper;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _ragdollHelper = new(gameObject);
            _ragdollHelper.Disable();
        }

        public void Initalize()
        {
            _weaponIK ??= GetComponentInChildren<WeaponViewIK>();
            Revive();
        }

        public virtual void Revive()
        {
            _animator.enabled = true;
            _weaponIK.DisablePhysicBehaviour();
            _ragdollHelper.Disable();
            PlayIdle();
        }

        public void PlayClipWithName(string clipName) => _animator.Play(clipName);
        public virtual void PlayIdle() => _animator.SetBool("IsFire", false);

        public virtual void PlayAttack() => _animator.SetBool("IsFire", true);

        public virtual void PlayMove() => _animator.SetFloat("Speed", 1);

        public virtual void PlayDead()
        {
            _weaponIK.EnablePhysicBehaviour();
            _weaponIK.transform.SetParent(transform);
            _animator.enabled = false;

            _ragdollHelper.Enable();
            _ragdollHelper.AddForceInDirection(-transform.forward + Vector3.up);
        }

        protected virtual void RefreshAnimator() => _animator.Rebind();

    }
}
