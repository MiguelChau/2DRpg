using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animBoolName;
    public PlayerStates(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = animBoolName;   
    }

    public virtual void Enter()
    {

    }
    
    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
