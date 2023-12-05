using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estado que representa o estado do jogador quando está no chao e parado. Main Default no Animator
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter() //Chama a função zeroVelocity
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.isWallDetected()) //Se a tecla de direçao for pressionado na direçao da parede em que o jogador esta a colidir, nao faz nada.
            return; //para o player nao correr na parede

        if (xInput != 0 && !player.isBusy) //Se a tecla de direção for diferente de zero e o jogador não estiver ocupado, muda para o estado PlayerMoveState.
            stateMachine.ChangeState(player.moveState);
    }
}
