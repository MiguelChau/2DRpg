using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyStates
{
    private Enemy_Shady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Shady enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            enemy.SelfDestroy();

    }
}
