using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemDataEquipement _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int j = 0; j < _data.craftMaterials.Count; j++)
        {
            if (_data.craftMaterials.Count > materialImage.Length)
                Debug.Log("You have more materials amount than have material slots in craft window");

            materialImage[j].sprite = _data.craftMaterials[j].data.icon;
            materialImage[j].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[j].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftMaterials[j].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(_data, _data.craftMaterials));
    }
}
