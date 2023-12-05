using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Representa o estado do jogador quando salta
public class PlayerJumpState : PlayerStates
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce); //Define a velocidade Y do jogador para a for�a de pulo
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0) //Se a velocidade Y do jogador se tornar negativa, significa que o jogador est� descendo ap�s o ponto mais alto do salto
            stateMachine.ChangeState(player.airState); //Nesse caso, muda para o estado de AirState.
    }
}
