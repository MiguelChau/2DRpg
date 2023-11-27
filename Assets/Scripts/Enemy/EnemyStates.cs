using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemy;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyStates(Enemy _enemy, EnemyStateMachine enemyStateMachine, string animBoolName)
    {
        this.enemy = _enemy;
        this.enemyStateMachine = enemyStateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
