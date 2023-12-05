using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estado que repsentar o estado do jogador quando ele esta no chao movendo-se
public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y); //Define a velocidade do jogador com base no X e na velocidade Y
        base.Update();


        if (xInput == 0 || player.isWallDetected()) //Se a entrada for zero ou se o jogador estiver a colidir com a parede muda para o IdleState
            stateMachine.ChangeState(player.idleState);
    }
}
