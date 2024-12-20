using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TurnToPlayer", story: "[Enemy] turn to [Player]", category: "Action", id: "3c45bf71f7be59759b8cbcf2e2fe3a1b")]
public partial class TurnToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> _angleThreshold;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!IsLookingAt())
        {
            ProcessAiming();
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }

    private void ProcessAiming()
    {
        Vector3 direction = Player.Value.transform.position - Enemy.Value.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 eulerAngles = lookRotation.eulerAngles;

        eulerAngles.x = 0;

        lookRotation = Quaternion.Euler(eulerAngles);

        Enemy.Value.transform.rotation = Quaternion.Slerp(Enemy.Value.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private bool IsLookingAt()
    {
        Vector3 directionToTarget = Player.Value.transform.position - Enemy.Value.transform.position;

        directionToTarget.Normalize();
        Vector3 directionToTargetFlat = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;
        Vector3 enemyForwardFlat = new Vector3(Enemy.Value.transform.forward.x, 0, Enemy.Value.transform.forward.z).normalized;

        float angle = Vector3.Angle(enemyForwardFlat, directionToTargetFlat);

        return angle <= _angleThreshold.Value;
    }
}

