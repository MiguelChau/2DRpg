using System.Collections.Generic;
using UnityEngine;

public enum EquipementType
{
    Weapon,
    Armor,
    Necklace,
    Flask
}

[CreateAssetMenu(fileName = "New Equipement Data", menuName = "Data/Equipement")]
public class ItemDataEquipement : ItemData
{
    public EquipementType equipementType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength;
    public int agility; 
    public int intelligence; 
    public int vitality;
    [Space]
    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower; 
    [Space]
    [Header("Defensive Sats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftMaterials;

    private int descriptionLength;

    public void Effect(Transform _enemyPos)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPos);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.health.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.health.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resis.");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Thunder Damage");

        if(descriptionLength < 5 )
        {
            for(int i = 0; i < 5 - descriptionLength; ++i) { }
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if(_value > 0)
                sb.Append("+" + _value + " " + _name);

            descriptionLength++;
        }
    }
}
