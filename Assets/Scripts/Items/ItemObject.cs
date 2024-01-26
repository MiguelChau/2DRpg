using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItemDrop(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickUpItem()
    {
        if (!Inventory.Instance.CanAddItem() && itemData.itemType == ItemType.Equipement)
        {
            rb.velocity = new Vector2(0, 7);
            PlayerManager.instance.player.fx.CreatePopUp("Inventory is Full");
            return;
        }

        AudioManager.Instance.PlaySFX(9, transform);
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
