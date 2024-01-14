using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameTexT;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;
    public void ShowToolTip(ItemDataEquipement item)
    {
        if (item == null)
            return;

        itemNameTexT.text = item.itemName;
        itemTypeText.text = item.equipementType.ToString();
        itemDescription.text = item.GetDescription();


        AdjustFontSize(itemNameTexT);
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameTexT.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
