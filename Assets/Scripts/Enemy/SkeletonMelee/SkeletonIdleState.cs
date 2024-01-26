using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton enemy) : base(_enemyBase, enemyStateMachine, animBoolName, enemy)
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
