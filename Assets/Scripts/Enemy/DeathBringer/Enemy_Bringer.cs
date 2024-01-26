using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bringer : Enemy
{
    public bool bossFightBegun; //responsavel pro fazer começar a batalha


    [Header("Teleport Info")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;

    [Header("SpellCast Info")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellCastOffSet;

    public float spellCooldown;
    public int amountOfSpells;
    public float lastTimeCast;


    #region STATES
    public BringerIdleState idleState { get; private set; }
    public BringerTeleportState teleportState { get; private set; }
    public BringerBattleState battleState { get; private set; }
    public BringerAttackState attackState { get; private set; }
    public BringSpellCastState spellCastState { get; private set; }
    public BringerDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new BringerIdleState(this, stateMachine, "Idle", this);
        teleportState = new BringerTeleportState(this, stateMachine, "Teleport", this);
        battleState = new BringerBattleState(this, stateMachine, "Move", this);
        attackState = new BringerAttackState(this, stateMachine, "Attack", this);
        spellCastState = new BringSpellCastState(this, stateMachine, "SpellCast", this);
        deadState = new BringerDeadState(this, stateMachine, "Idle", this);
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


    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffSet = 0;

        if (player.rb.velocity.x != 0)
            xOffSet = player.facingDir * spellCastOffSet.x;

        Vector3 spellPos = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + spellCastOffSet.y);

        GameObject newSpell = Instantiate(spellPrefab, spellPos, Quaternion.identity);
        newSpell.GetComponent<SpellCastController>().SetupSpellCast(stats);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsule.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            // Debug.Log("Looking for new position");
            FindPosition();
        }
    }


    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if(Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }

        return false;
    }
}
