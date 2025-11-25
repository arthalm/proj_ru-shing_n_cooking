using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Text inventoryText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInventory.CurrentItem != null)
        {
            inventoryImage.enabled = true;
            inventoryImage.sprite = playerInventory.CurrentItem.Icon;
        }
        else
        {
            inventoryImage.enabled = false;
        }
    }

    public void SetCurrentItem(Sprite icon, string itemName)
    {
        inventoryImage.sprite = icon;
        inventoryText.text = "Item no inventário " + itemName;
    }
}
