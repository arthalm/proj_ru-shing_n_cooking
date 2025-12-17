using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private Item item;
    public Item ItemData
    {
        get => item;
        set => item = value;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.root.CompareTag("Player"))
        {
            PlayerInventory playerInventory = collider.transform.root.GetComponent<PlayerInventory>();
            playerController = collider.transform.root.GetComponent<PlayerController>();
            if (playerInventory != null)
            {
                if (!playerInventory.HasItem)
                {
                    playerInventory.AddItem(ItemData);
                    Destroy(gameObject);
                }
                else
                {
                    playerController.SetNearbyItem(this);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.root.CompareTag("Player") && playerController != null && playerController.NearbyItem == this)
        {
            playerController.SetNearbyItem(null);
        }
    }
}