using UnityEngine;

namespace EndlessRoad
{
    public class EnemyRange : EnemyBase
    {
        [SerializeField] protected WeaponView _weaponView;
        public override void Initialize()
        {
            _combat.Initialize(_weaponConfig, _weaponView);
            _animator.Initalize();
            _agent.enabled = true;
        }
    }
}