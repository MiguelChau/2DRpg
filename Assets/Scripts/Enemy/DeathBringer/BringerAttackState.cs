using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAttackState : EnemyStates
{
    private Enemy_Bringer enemy;

    public BringerAttackState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Bringer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
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
        {
            if (enemy.CanTeleport())
                enemyStateMachine.ChangeState(enemy.teleportState);
            else
                enemyStateMachine.ChangeState(enemy.battleState);
        }
    }
}
