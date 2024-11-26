using UnityEngine;

namespace EndlessRoad
{
    public class AmmoTrail : AmmoBase
    {
        protected override void DoImpactToPoint(RaycastHit hit)
        {
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent(out IImpactable impactable))
                {
                    _impactService.DoImpactToTargetPoint(impactable, hit.point);
                    if (impactable is IDamageable damageable)
                    {
                        damageable.ApplyDamage(_damage);
                    }
                }
            }
        }
    }
}
