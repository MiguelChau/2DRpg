using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady Info")]
    public float battleMoveSpeed;

    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region STATES
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunnedState stunState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
        stunState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
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
        GameObject newExplosive = Instantiate(explosivePrefab, attackCheck.position, Quaternion.identity);

        newExplosive.GetComponent<ExplosiveController>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        capsule.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}
