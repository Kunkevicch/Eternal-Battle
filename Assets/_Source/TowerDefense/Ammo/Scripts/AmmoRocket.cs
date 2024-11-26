using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class AmmoRocket : AmmoBase
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private float _damageRadius;
        [SerializeField] private LayerMask _impactLayerMask;
        [SerializeField] private int _maxAffectedEnemies;

        [Inject]
        private ObjectPool _objectPool;

        protected override void DoImpactToPoint(RaycastHit hit)
        {
            IExplosion explosion = (IExplosion)_objectPool.ReuseComponent(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.InitializeExplosion(_damage, _damageRadius, _maxAffectedEnemies, _impactLayerMask);
            explosion.ActivateExplosion();
        }
    }
}
