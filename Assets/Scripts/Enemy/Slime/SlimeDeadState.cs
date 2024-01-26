using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyStates
{
    private Enemy_Slime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Slime enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.capsule.enabled = false;

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);

    }
}
