using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyStates
{
    private Transform player; //referencia ao jogador
    private  EnemySkeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemySkeleton _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
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

        if(enemy.IsPlayerDetected()) //verifica se há detecçao
        {
            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance < enemy.attackDist) //se o jogador estiver dentro das distancia, verifica se pode atacar
            {
                if(CanAttack())
                    enemyStateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10) //se nao estiver na distancia e tiver fora de alcance, fica no estado idle
                enemyStateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x) //atualiza a direçao do movimento do inimigo em direçao ao jogador
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack() //verifica se o inimigo pode atacar consoante o tempo de cooldown
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCD, enemy.maxAttackCD);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        Debug.Log("Attack CD");
        return false;
    }
}

