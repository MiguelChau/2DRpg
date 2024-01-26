public class BringerTeleportState : EnemyStates
{
    private Enemy_Bringer enemy;
    public BringerTeleportState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Bringer _enemy) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.InvencibleTime(true);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
                enemyStateMachine.ChangeState(enemy.spellCastState);
            else
                enemyStateMachine.ChangeState(enemy.battleState);
        }      
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.InvencibleTime(false);
    }
}


