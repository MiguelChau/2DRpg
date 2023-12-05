using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whereIsPlayer; //Para detectar o player

    [Header("Stunned Info")]
    public float stunnedDur; //duração do stun
    public Vector2 stunnedDir; //direçao do stun
    protected bool canBeStunned; //indicação se pode ser stunned
    [SerializeField] protected GameObject counterImage; //referencia a um objecto de imagem para o counterattack

    [Header("Move Info")]
    public float moveSpeed; //velocidade de movimento
    public float idleTime; //tempo parado
    public float battleTime; //tempo em batalha

    [Header("Attack Info")]
    public float attackDist;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked; //para manter o tempo do ultimo ataque


    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {

        base .Update();

        stateMachine.currentState.Update();

    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned= false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned() //verifica se o inimigo pode ser stunned
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDist * facingDir, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whereIsPlayer);
    // para detectar o player na direçao do inimigo
}
