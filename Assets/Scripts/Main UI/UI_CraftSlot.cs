using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{


    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemDataEquipement _data)
    {
        if(_data == null) 
            return;

        item.data = _data; //este item.data vem do script de uiitemslot

        itemImage.sprite = _data.icon;
        itemText.text = _data.name;

        if (itemText.text.Length > 12)
            itemText.fontSize = itemText.fontSize * .7f;
        else
            itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipement);
    }
}
