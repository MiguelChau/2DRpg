using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estado que representa o estado do jogador quando est� no chao e parado. Main Default no Animator
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter() //Chama a fun��o zeroVelocity
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

        if (xInput == player.facingDir && player.isWallDetected()) //Se a tecla de dire�ao for pressionado na dire�ao da parede em que o jogador esta a colidir, nao faz nada.
            return; //para o player nao correr na parede

        if (xInput != 0 && !player.isBusy) //Se a tecla de dire��o for diferente de zero e o jogador n�o estiver ocupado, muda para o estado PlayerMoveState.
            stateMachine.ChangeState(player.moveState);
    }
}
