using UnityEngine;

public class PlayerCounterAttackState : PlayerStates
{
    private bool createClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        createClone = true;
        stateTimer = player.counterAttackDur;
        player.anim.SetBool("SucessCounter", false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {

            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SucessfulCounter();
            }


            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SucessfulCounter();

                    player.skill.parry.UseSkill();

                    if (createClone)
                    {
                        createClone = false;
                        player.skill.parry.DoMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void SucessfulCounter()
    {
        stateTimer = 10;
        player.anim.SetBool("SucessCounter", true);
    }
}
