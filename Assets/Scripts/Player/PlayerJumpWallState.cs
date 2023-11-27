using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estado que representa o estado do jogaodr quando ele salta em uma parede
public class PlayerJumpWallState : PlayerStates
{
    public PlayerJumpWallState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .4f;
        player.SetVelocity(5 * - player.facingDir, player.jumpForce); //velocidade inicial para o salto na diraçao oposta à parede
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update() //reduz o temporizador de estado
    {
        base.Update();

        if (stateTimer < 0) //se o temporizador acabar muda para o airState
            stateMachine.ChangeState(player.airState);

        if (player.IsGroundDetected()) //Se o jogador detectar solo, muda para o IdleState
            stateMachine.ChangeState(player.idleState);
    }

}
