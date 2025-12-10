using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Item carriedItem;
    public bool HasItem => CurrentItem != null;
    public Item CurrentItem => carriedItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>();
        }
        if (inventoryUI != null)
        {
            inventoryUI.UpdateHud();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddItem(Item item)
    {
        carriedItem = item;
        inventoryUI.UpdateHud();
    }

    public void RemoveItem()
    {
        carriedItem = null;
        inventoryUI.UpdateHud();
    }
}