using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherGroundState : EnemyStates
{
    protected Transform player;
    protected Enemy_Archer enemy;

    public ArcherGroundState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Archer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.aggroDist)
            enemyStateMachine.ChangeState(enemy.battleState);
    }

}
