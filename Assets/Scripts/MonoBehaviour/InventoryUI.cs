using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Image inventoryImage;
    [SerializeField] private GameObject hudObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideHUD();
        UpdateHud();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHud()
    {
        if (playerInventory.CurrentItem != null)
        {
            ShowHUD();
            inventoryImage.sprite = playerInventory.CurrentItem.Icon;
        }
        else
        {
            HideHUD();
        }
    }

    public void ShowHUD()
    {
        hudObject.SetActive(true);
    }

    public void HideHUD()
    {
        hudObject.SetActive(false);
    }
}