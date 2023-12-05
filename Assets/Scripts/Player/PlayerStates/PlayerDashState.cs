using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerStates
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter() //inicia o temporizador de estado
    {
        base.Enter();

        player.skill.clone.CreateClone(player.transform); //usando o singleton de skill manager

        stateTimer = player.dashDur; 
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.isWallDetected()) 
            stateMachine.ChangeState(player.wallSlideState); //muda para o estado de playerwallstate se o jgoador nao estiver no chao e estiver a colidir com uma parede

        player.SetVelocity(player.dashSpeed * player.dashDir, 0); //define a velocidade do jogador para o dash

        Debug.Log("Dash onfoward");

        if (stateTimer < 0) //quando acabar o temporizador muda para o idlestate
            stateMachine.ChangeState(player.idleState);
    }
}
