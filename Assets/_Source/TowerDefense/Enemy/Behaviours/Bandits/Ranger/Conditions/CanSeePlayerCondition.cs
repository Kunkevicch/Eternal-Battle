using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanSeePlayer", story: "[Enemy] can see [Player] in [DistanceToPlayer]", category: "Conditions", id: "3c657e8079c94ffebb07281a48107825")]
public partial class CanSeePlayerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> DistanceToPlayer;

    public override bool IsTrue()
    {
        Vector3 direction = Player.Value.transform.position - Enemy.Value.transform.position;

        if (Physics.Raycast(Enemy.Value.transform.position, direction, out RaycastHit hitinfo, DistanceToPlayer, Enemy.Value.VisiubleLayer))
        {
            return hitinfo.collider.gameObject.IsInLayer(LayerMask.GetMask("Player"));
        }

        return false;
    }
}
