using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStunnedState : EnemyStates
{
    private Enemy_Archer enemy;

    public ArcherStunnedState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Archer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
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

        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.idleState);
    }
}
