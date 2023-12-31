using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null )
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if(statType == StatType.Health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            
            if(statType == StatType.Damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.CritPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.CritChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.Evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.MagicRes)
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();

            if (statType == StatType.FireDamage)
                statValueText.text = (playerStats.fireDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();

            if (statType == StatType.IceDamage)
                statValueText.text = (playerStats.iceDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();

            if (statType == StatType.LightiningDamage)
                statValueText.text = (playerStats.lightningDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
