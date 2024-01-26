using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType
{
    Big,
    Medium,
    Small
}
public class Enemy_Slime : Enemy
{
    [Header("Slime Kind")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region STATES
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);

        stunState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
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

        if (slimeType == SlimeType.Small)
            return;

        CreateSlimes(slimesToCreate, slimePrefab);
    }

    private void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for(int i = 0;  i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir)
            Flip();

        float xVel = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVel = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel * -facingDir, yVel);

        Invoke("CancelKnockBack", 1.5f);
    }

    private void CancelKnockBack() => isKnocked = false;
}
