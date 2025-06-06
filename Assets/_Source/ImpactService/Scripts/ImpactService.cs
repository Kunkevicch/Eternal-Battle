using UnityEngine;

namespace EndlessRoad
{
    public class ImpactService
    {
        private readonly ObjectPool _objectPool;

        public ImpactService(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public void DoImpactToTargetPoint(IImpactable impactable, Vector3 point)
        {
            var impactEffect = (ImpactEffect)_objectPool.ReuseComponent(
                impactable.ImpactPrefab.gameObject
                , point
                , Quaternion.identity
                );
            impactEffect.gameObject.SetActive(true);
        }
    }
}
