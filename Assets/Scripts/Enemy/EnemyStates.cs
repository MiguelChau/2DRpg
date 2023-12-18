using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyStates(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.enemyStateMachine = enemyStateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
