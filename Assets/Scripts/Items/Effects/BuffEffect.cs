using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Health,
    Armor,
    Evasion,
    MagicRes,
    FireDamage,
    IceDamage,
    LightiningDamage
}

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/ItemEffect/Buff")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDur;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDur, StatToModify());
    }

    private Stat StatToModify()
    {
        if (buffType == StatType.Strength)
            return stats.strength;
        else if(buffType == StatType.Agility)
            return stats.agility;
        else if(buffType == StatType.Intelligence)
            return stats.intelligence;
        else if(buffType == StatType.Vitality)
            return stats.vitality;
        else if (buffType == StatType.Damage)
            return stats.damage;
        else if( buffType == StatType.CritChance)
            return stats.critChance;
        else if(buffType == StatType.CritPower)
            return stats.critPower;
        else if (buffType == StatType.Health)
            return stats.maxHealth;
        else if (buffType == StatType.Armor)
            return stats.armor;
        else if (buffType == StatType.Evasion)
            return stats.evasion;
        else if (buffType == StatType.MagicRes)
            return stats.magicResistance;
        else if (buffType == StatType.FireDamage)
            return stats.fireDamage;
        else if (buffType == StatType.IceDamage)
            return stats.iceDamage;
        else if (buffType == StatType.LightiningDamage)
            return stats.lightningDamage;

        return null;
    }
}
