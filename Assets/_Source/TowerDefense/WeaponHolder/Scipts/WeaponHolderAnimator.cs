using UnityEngine;

namespace EndlessRoad
{
    public class WeaponHolderAnimator
    {
        private readonly Animator _animator;
        private readonly RuntimeAnimatorController _defaultAnimatorController;

        private const string EQUIP_CLIP = "Equip";
        private const string FIRE_CLIP = "Fire";
        private const string RELOAD_START_CLIP = "ReloadStart";
        private const string RELOAD_END_CLIP = "ReloadEnd";

        public WeaponHolderAnimator(Animator animator)
        {
            _animator = animator;
            _defaultAnimatorController = _animator.runtimeAnimatorController;
        }

        public void SetAnimatorController(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
        }

        public void ResetAnimatorController() => _animator.runtimeAnimatorController = _defaultAnimatorController;

        public void PlayEquip() => _animator.Play(EQUIP_CLIP);
        public void PlayFire() => _animator.Play(FIRE_CLIP);
        public void PlayReloadStart() => _animator.Play(RELOAD_START_CLIP);
        public void PlayReloadEnd() => _animator.Play(RELOAD_END_CLIP);
    }
}
