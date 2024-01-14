using System.Collections;
using UnityEngine;

//Componente principal do jogador
public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement; //define os movimentos associados ao ataques
    public float counterAttackDur = .2f;
    public bool isBusy { get; private set; } //para indicar se o jogador está ocupado com alguma ação

    [Header("Movement")] //variaveis para movimento e salto
    public float moveSpeed = 5f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;


    [Header("Special Dash")] //variaveis para special dash
    public float dashSpeed;
    public float dashDur;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States 
    //Instancia os diferentes estados do jogador no metodo do Awake
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerJumpWallState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallJumpState = new PlayerJumpWallState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

        primaryAttackState = new PrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;


        stateMachine.Init(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();

        }


        if(Input.GetKeyDown(KeyCode.B))
        {
            Inventory.Instance.UseFlask();
        }
    }

    public override void SlowEntity(float _slowPercent, float _slowDur)
    {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("ReturnDefaultSpeed", _slowDur);
    }

    public override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _sec) //Coroutine para definir o jogador como ocupado
    {
        isBusy = true;

        yield return new WaitForSeconds(_sec);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if (isWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;

        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }


}
