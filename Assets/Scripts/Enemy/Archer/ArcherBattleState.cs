using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyStates
{
    private Transform player;
    private Enemy_Archer enemy;
    private int moveDir;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Archer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            enemyStateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected()) //verifica se há detecçao
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.safeDistance)
            {
                if (CanJump())
                    enemyStateMachine.ChangeState(enemy.jumpState);
            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDist) //se o jogador estiver dentro das distancia, verifica se pode atacar
            {
                if (CanAttack())
                    enemyStateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7) //se nao estiver na distancia e tiver fora de alcance, fica no estado idle
                enemyStateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipControl();
    }

    private void BattleStateFlipControl()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack() //verifica se o inimigo pode atacar consoante o tempo de cooldown
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCD, enemy.maxAttackCD);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    private bool CanJump()
    {
        if (enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true)
            return false;

        if(Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }    

        return false;
    }
}
