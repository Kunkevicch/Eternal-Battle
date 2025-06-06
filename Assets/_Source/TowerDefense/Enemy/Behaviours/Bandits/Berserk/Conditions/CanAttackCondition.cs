using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanAttack", story: "Is [Enemy] can attack", category: "Conditions", id: "71808e1198f338d3dce768ed108fb882")]
public partial class CanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;

    public override bool IsTrue() => Enemy.Value.CanAtack;

}
