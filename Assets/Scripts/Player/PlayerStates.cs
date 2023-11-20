using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    private string animBoolName;

    protected float stateTimer;
    public PlayerStates(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = animBoolName;   
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;

        Debug.Log("Enter " + animBoolName);
    }
    
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");

        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);

        Debug.Log("Exit " + animBoolName);
    }
}
