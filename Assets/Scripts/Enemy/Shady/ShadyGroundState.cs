using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyGroundState : EnemyStates
{
    protected Enemy_Shady enemy;
    protected Transform player;
    public ShadyGroundState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Shady _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.aggroDist)
            enemyStateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
