using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipementType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        Inventory.Instance.UnequipItem(item.data as ItemDataEquipement);
        Inventory.Instance.AddItem(item.data as ItemDataEquipement);
        CleanUpSlot();
    }
}
