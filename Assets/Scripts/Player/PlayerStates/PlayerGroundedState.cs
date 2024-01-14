using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este estado representa o estado do jogador quando ele está no chão(sem salto, nem paredes)
public class PlayerGroundedState : PlayerStates
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter() //ativaçao do modo Idle
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update() //verifica a entrada do jogador para mudar de estado.
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked) 
            stateMachine.ChangeState(player.blackHoleState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.Alpha2) && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetKeyDown(KeyCode.Alpha1)) //Se a tecla 1 for pressionado, muda o estado para ataque primário
            stateMachine.ChangeState(player.primaryAttackState);

        if (!player.IsGroundDetected()) //Se nao existir detençao de solo, muda de estado para airState
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) //Quando pressionado Space e existir detençao de solo, muda para o State de jump
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
