using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private Item itemData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.transform.root.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                if (!inventory.hasItem)
                {
                    inventory.AddItem(itemData);
                    Destroy(gameObject);
                }
            }
        }
    }
}