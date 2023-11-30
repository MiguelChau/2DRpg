using System.Collections;
using UnityEngine;

//Componente principal do jogador
public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement; //define os movimentos associados ao ataques
    public bool isBusy { get; private set; } //para indicar se o jogador está ocupado com alguma ação

    [Header("Movement")] //variaveis para movimento e salto
    public float moveSpeed = 5f;
    public float jumpForce;

    [Header("Special Dash")] //variaveis para special dash
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDur;
    public float dashDir { get; private set; }

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

        primaryAttackState = new PrimaryAttackState(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Init(idleState);

    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        CheckDashInput();
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

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }


}
