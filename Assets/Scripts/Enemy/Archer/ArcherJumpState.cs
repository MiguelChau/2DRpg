using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyStates
{
    private Enemy_Archer enemy;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Archer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy= _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(enemy.jumpVel.x * -enemy.facingDir, enemy.jumpVel.y);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && enemy.IsGroundDetected())
            enemyStateMachine.ChangeState(enemy.battleState);
    }
}
