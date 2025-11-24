using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Item carriedItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (carriedItem == null)
        {
            AddItem(carriedItem);
        }
    }
    public void AddItem(Item item)
    {
        carriedItem = item;
    }

    public void RemoveItem(Item item)
    {
        
    }
}
