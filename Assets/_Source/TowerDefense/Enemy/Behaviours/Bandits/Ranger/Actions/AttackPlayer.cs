using EndlessRoad;
using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackPlayer", story: "[Enemy] attack [Player] every [AttackTime] with [TimeBetweenAttacks]", category: "Action", id: "08c379e06ac715d15f547c6ad875d798")]
public partial class ShootInPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyBase> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> AttackTime;
    [SerializeReference] public BlackboardVariable<float> TimeBetweenAttacks;
    [SerializeReference] public BlackboardVariable<bool> Continuous = new BlackboardVariable<bool>(false);

    private bool _isAttacking;
    private float timer = 0f;
    protected override Status OnStart()
    {
        if (Continuous.Value)
        {
            Enemy.Value.StartCombatProcess();
            return Status.Running;
        }
        else
        {
            Enemy.Value.CombatProcess();
            return Status.Success;
        }
    }

    protected override Status OnUpdate()
    {
        if (Continuous.Value)
        {
            if (_isAttacking)
            {
                // ���� �����
                Enemy.Value.CombatProcess();
                timer += Time.deltaTime;
                if (timer >= AttackTime.Value)
                {
                    _isAttacking = false;
                    timer = 0f;
                }
            }
            else
            {
                // ���� ��������
                timer += Time.deltaTime;
                if (timer >= TimeBetweenAttacks.Value)
                {
                    _isAttacking = true;
                    timer = 0f;
                }
            }
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }
}

