using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundState
{
    public SlimeMoveState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Slime _enemy) : base(_enemyBase, enemyStateMachine, animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.isWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            enemyStateMachine.ChangeState(enemy.idleState);
        }
    }
}
