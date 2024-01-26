using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerIdleState : EnemyStates
{
    private Enemy_Bringer enemy;
    private Transform player;

    public BringerIdleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Bringer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegun = true;

        if (Input.GetKeyDown(KeyCode.J))
            enemyStateMachine.ChangeState(enemy.teleportState);

        if (stateTimer < 0 && enemy.bossFightBegun)
            enemyStateMachine.ChangeState(enemy.battleState);
    }
}
