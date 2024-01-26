using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer Info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowDamage;

    public Vector2 jumpVel;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("BackFlip Collision Check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindSize;

    #region STATES
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunnedState stunState { get; private set; }
    public ArcherDeathState deadState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();


        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ArcherDeathState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned() //subscreve o metodo CanbeStunned herdado do Enemy.cs
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<Arrow_Controller>().SetupArrow(arrowSpeed * facingDir, stats);

    }

    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindSize, 0, Vector2.zero, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDis + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindSize);
    }

}
