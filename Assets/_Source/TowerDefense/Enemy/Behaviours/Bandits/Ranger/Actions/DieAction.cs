using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Die", story: "[Enemy] died", category: "Action", id: "9a0815428e09a2b3259dcee43fb163a7")]
public partial class DieAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;

    protected override Status OnStart()
    {
        Enemy.Value.DeadProcess();
        return Status.Success;
    }
}

