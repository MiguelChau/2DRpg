using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyStates
{
    private Enemy_Slime enemy;

    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Slime _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;

    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            enemyStateMachine.ChangeState(enemy.battleState);
    }
}
