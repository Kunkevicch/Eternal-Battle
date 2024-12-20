using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanRage", story: "[EnemyBerserk] can rage", category: "Conditions", id: "9584c7d5b1c9c88414443ee418b9da44")]
public partial class CanRageCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyBerserk> EnemyBerserk;

    public override bool IsTrue()
    {
        return EnemyBerserk.Value.CanRage;
    }
}
