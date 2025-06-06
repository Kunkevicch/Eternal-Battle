using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsEnemyDead", story: "Is [Enemy] dead", category: "Conditions", id: "f232a2b2bf4f3a84bf4271377203f95f")]
public partial class IsEnemyDeadCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;

    public override bool IsTrue()
    {
        return Enemy.Value.IsDead;
    }
}
