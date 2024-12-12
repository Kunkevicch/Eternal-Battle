using UnityEngine;

namespace EndlessRoad
{
    public class WeaponViewMelee : WeaponView
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRange;

        public override void Initialize(LayerMask impactMask, WeaponConfig weaponConfig, ShootRaycastStrategyBase shootStrategy)
        {
            _impactMask = impactMask;
            _damage = weaponConfig.Damage;
        }
        protected override void Attack()
        {
            var attackInfo = Physics.OverlapSphere(_attackPoint.position, _attackRange, _impactMask);
            
            foreach (var attack in attackInfo)
            {
                if (attack.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(_damage);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
}
