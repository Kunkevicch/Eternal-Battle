using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CalculateDistance", story: "Calculate distance between [self] and [player] and save it to [DistanceToPlayer]", category: "Action", id: "16bf67fa21ffad1d29213b098af1a3ec")]
public partial class CalculateDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> DistanceToPlayer;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        DistanceToPlayer.Value = Vector3.Distance(Self.Value.transform.position, Player.Value.transform.position);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

