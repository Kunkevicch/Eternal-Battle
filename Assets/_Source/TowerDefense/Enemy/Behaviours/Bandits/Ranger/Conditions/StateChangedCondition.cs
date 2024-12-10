using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "StateChanged", story: "if [CurrentState] changed", category: "Conditions", id: "ea99f7f7cd7f7a7cd3cb3886aa96e541")]
public partial class StateChangedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<State> CurrentState;
    private State _prevState;

    public override bool IsTrue()
    {
        return _prevState != CurrentState.Value;
    }

    public override void OnStart()
    {
        _prevState = CurrentState.Value;
    }

    public override void OnEnd()
    {
    }
}
