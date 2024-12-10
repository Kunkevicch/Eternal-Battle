using UnityEngine;

namespace EndlessRoad
{
    public class EnemyRanger : EnemyBase
    {
        public override void Initialize()
        {
            _combat.Initialize(_weaponConfig);
            _animator.Initalize();
            _agent.enabled = true;
        }
    }
}