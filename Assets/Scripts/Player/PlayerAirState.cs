using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Representa o estado do jogador quando ele está no ar
public class PlayerAirState : PlayerStates
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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
        base.Update();

        if (player.isWallDetected()) //Se o jogador estiver a colidir com uma parede no ar, muda para o estado de wallSlideState
            stateMachine.ChangeState(player.wallSlideState);

        if(player.IsGroundDetected()) //Se o jogador detectar solo muda para o idleState
            stateMachine.ChangeState(player.idleState);

        if(xInput != 0) //Mantem uma velocidade horizontal reduzida se houver entrada vertical
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
