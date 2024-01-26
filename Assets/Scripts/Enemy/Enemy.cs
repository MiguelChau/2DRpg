using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whereIsPlayer; //Para detectar o player

    [Header("Stunned Info")]
    public float stunnedDur = 1; //duração do stun
    public Vector2 stunnedDir = new Vector2(10, 12); //direçao do stun
    protected bool canBeStunned; //indicação se pode ser stunned
    [SerializeField] protected GameObject counterImage; //referencia a um objecto de imagem para o counterattack

    [Header("Move Info")]
    public float moveSpeed = 2; //velocidade de movimento
    public float idleTime = 2; //tempo parado
    public float battleTime = 7; //tempo em batalha
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float aggroDist = 2;
    public float attackDist = 2;
    public float attackCooldown;
    public float minAttackCD = 1;
    public float maxAttackCD = 2;
    [HideInInspector] public float lastTimeAttacked; //para manter o tempo do ultimo ataque


    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    private Player player;
    public string lastAnimBoolName { get; private set; } //para parar a animaçao

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<EntityFX>();
    }
    protected override void Update()
    {

        base.Update();

        stateMachine.currentState.Update();

    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntity(float _slowPercent, float _slowDur)
    {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("ReturnDefaultSpeed", _slowDur);
    }

    public override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }
    public virtual void FreezeTime(bool _timeFrozen) //metodo para desacelarar/congelar o inimigo. No caso, a velocidade de movimento e animação estao como 0 para simular a desacelaraçao.
    {
        if (_timeFrozen)
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

    public virtual void FreezeTimeFor(float _dur) => StartCoroutine(FreezeTimeCoroutine(_dur));

    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
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
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned() //verifica se o inimigo pode ser stunned
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }
    #endregion

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDist * facingDir, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whereIsPlayer);

    // para detectar o player na direçao do inimigo
}
