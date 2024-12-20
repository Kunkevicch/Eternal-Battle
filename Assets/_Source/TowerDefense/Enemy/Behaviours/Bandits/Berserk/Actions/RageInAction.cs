using EndlessRoad;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RageIn", story: "[EnemyBerserk] goes berserk rage", category: "Action", id: "7d7a4bf592d8df526d0b6bba80189e5a")]
public partial class RageInAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyBerserk> EnemyBerserk;
    [SerializeReference] public BlackboardVariable<bool> IsInRageStatus;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    private bool _isRaging;
    protected override Status OnStart()
    {
        IsInRageStatus.Value = true;
        _isRaging = true;
        EnemyBerserk.Value.StartRageProcess(() => OnRageComplete(), () => OnRageEnd());
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_isRaging)
        {
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }

    private void OnRageComplete()
    {
        MoveSpeed.Value = EnemyBerserk.Value.MoveSpeed;
        _isRaging = false;
    }

    private void OnRageEnd()
    {
        MoveSpeed.Value = EnemyBerserk.Value.MoveSpeed;
        IsInRageStatus.Value = false;
    }
}

