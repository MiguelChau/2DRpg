using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerStates
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePos.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < mousePos.x && player.facingDir == -1)
            player.Flip();
    }
}
