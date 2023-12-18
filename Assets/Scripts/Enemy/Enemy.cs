using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whereIsPlayer; //Para detectar o player

    [Header("Stunned Info")]
    public float stunnedDur; //dura��o do stun
    public Vector2 stunnedDir; //dire�ao do stun
    protected bool canBeStunned; //indica��o se pode ser stunned
    [SerializeField] protected GameObject counterImage; //referencia a um objecto de imagem para o counterattack

    [Header("Move Info")]
    public float moveSpeed; //velocidade de movimento
    public float idleTime; //tempo parado
    public float battleTime; //tempo em batalha
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDist;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked; //para manter o tempo do ultimo ataque


    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName {  get; private set; } //para parar a anima�ao

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
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

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool _timeFrozen) //metodo para desacelarar/congelar o inimigo. No caso, a velocidade de movimento e anima��o estao como 0 para simular a desacelara�ao.
    {
        if(_timeFrozen)
        {
            moveSpeed = 0f;
            anim.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed; 
            anim.speed = 1f;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region CounterAttackWindow
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
    #endregion

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDist * facingDir, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whereIsPlayer);
    // para detectar o player na dire�ao do inimigo
}
