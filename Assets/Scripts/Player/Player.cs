using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

//Componente principal do jogador
public class Player : MonoBehaviour
{
    [Header("Attack Details")]
    public Vector2[] attackMovement; //define os movimentos associados ao ataques
    public bool isBusy {  get; private set; } //para indicar se o jogador está ocupado com alguma ação

    [Header("Movement")] //variaveis para movimento e salto
    public float moveSpeed = 5f;
    public float jumpForce;

    [Header("Special Dash")] //variaveis para special dash
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDur;
    public float dashDir { get; private set; }

    [Header("Collision")] //variaveis de detenção de colisao, do chão e das paredes
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDis;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDis;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States 
    //Instancia os diferentes estados do jogador no metodo do Awake
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerJumpWallState wallJumpState {  get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PrimaryAttackState primaryAttackState { get; private set; }    
    #endregion

    private void Awake()
    {
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

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Init(idleState);

    }

    private void Update()
    {
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
    #region Velocity

    //Métodos para manipular a velocidade, verificar colisões e inverter a direção do jogador.
    public void ZeroVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipRotate(_xVelocity);
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, whatIsGround);
    public bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDis, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDis, wallCheck.position.y));
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipRotate(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion
}
