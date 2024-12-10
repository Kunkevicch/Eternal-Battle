using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StartMove", story: "[Enemy] start move", category: "Action", id: "0f338efbe5b80df00116aa0b28befd49")]
public partial class StartMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;

    protected override Status OnStart()
    {
        Enemy.Value.StartMove();
        return Status.Success;
    }
}

