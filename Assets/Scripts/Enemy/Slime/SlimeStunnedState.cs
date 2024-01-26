using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyStates
{
    private Enemy_Slime enemy;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Slime enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunnedDur;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunnedDir.x, enemy.stunnedDir.y);
    }

    public override void Exit()
    {
        base.Exit();

        
        enemy.stats.InvencibleTime(false);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.fx.Invoke("CancelColorChange", 0);
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.InvencibleTime(true);
        }

        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.idleState);
    }
}
