using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region COMPONENTS
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }  
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsule { get; private set; }
    #endregion

    [Header("Knockback")]
    [SerializeField] protected Vector2 knockBackDirection;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    [Header("Collision")] //variaveis de detenção de colisao, do chão e das paredes
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDis;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDis;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    public System.Action onFlipped; //event

    protected virtual void Awake()
    {

    }
    
    protected virtual void Start()
    {
        sr= GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        capsule = GetComponent<CapsuleCollider2D>();

    }
    protected virtual void Update()
    {

    }

    public virtual void SlowEntity(float _slowPercent, float _slowDur)
    {
        Debug.Log("slow freeze");
    }

    public virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockBack");

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    #region Velocity

    //Métodos para manipular a velocidade, verificar colisões e inverter a direção do jogador.
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipRotate(_xVelocity);
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public void FlipRotate(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, whatIsGround);
    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDis, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDis, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

 

    public virtual void Die()
    {

    }
}
