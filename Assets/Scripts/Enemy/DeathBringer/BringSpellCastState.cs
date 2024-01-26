using UnityEngine;


public class BringSpellCastState : EnemyStates
{
    private Enemy_Bringer enemy;

    private int amountOfSpells;
    private float spellTimer;

    public BringSpellCastState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Bringer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.InvencibleTime(true);
        amountOfSpells = enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();


        if(amountOfSpells <= 0)
            enemyStateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.InvencibleTime(false);
        enemy.lastTimeCast = Time.time;
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
