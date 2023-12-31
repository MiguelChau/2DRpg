using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop enemyDrop;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentModifier = 0.4f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        enemyDrop = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critPower);
        Modify(critChance);

        Modify(health);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

    }

    private void Modify(Stat _stat)
    {
        for(int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier)); 
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        enemyDrop.GenerateDrop();
    }
}
