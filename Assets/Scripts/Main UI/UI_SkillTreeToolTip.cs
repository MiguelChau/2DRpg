using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTreeToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] protected float defaultNameFontSize;

    public void ShowToolTip(string _skillDescription, string _skillName, int _skillCost)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        skillCost.text = "Level " + _skillCost;

        AdjustPosition();
        AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
