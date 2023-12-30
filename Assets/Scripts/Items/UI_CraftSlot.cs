using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipement craftData = item.data as ItemDataEquipement;

        Inventory.Instance.CanCraft(craftData, craftData.craftMaterials);
    }
}
