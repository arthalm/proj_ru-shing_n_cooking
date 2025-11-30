using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item carriedItem;
    public bool hasItem => CurrentItem != null;
    public Item CurrentItem => carriedItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddItem(Item item)
    {
        if (carriedItem == null)
        {
            carriedItem = item;
        }
    }

    public void RemoveItem()
    {
        carriedItem = null;
    }
}