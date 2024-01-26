using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Slime _enemy) : base(_enemyBase, enemyStateMachine, animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.moveState);
    }
}
