using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AimToPlayer", story: "[Self] aiming to [Player]", category: "Action", id: "42e77d06e4ba8fd375e22d7fc4f2f5ed")]
public partial class AimToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<bool> Continuous = new BlackboardVariable<bool>(false);

    protected override Status OnStart()
    {
        ProcessAiming();
        return Continuous.Value ? Status.Running : Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (Continuous.Value)
        {
            ProcessAiming();
            return Status.Running;
        }
        return Status.Success;
    }

    private void ProcessAiming()
    {
        Vector3 direction = Player.Value.transform.position - Self.Value.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 eulerAngles = lookRotation.eulerAngles;

        eulerAngles.x = 0;

        lookRotation = Quaternion.Euler(eulerAngles);

        Self.Value.transform.rotation = Quaternion.Slerp(Self.Value.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

