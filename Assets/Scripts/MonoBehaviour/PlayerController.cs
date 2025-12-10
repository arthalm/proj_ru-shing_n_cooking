using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 lastDirection;
    private Item handItem;
    private Item floorItem;
    private CollectibleItem nearbyItem;
    [SerializeField] private float speed = 0.2f;
    public CollectibleItem NearbyItem => nearbyItem;
    Rigidbody2D rb;
    PlayerInventory playerInventory;
    Animator anim;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInventory = GetComponent<PlayerInventory>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        anim.SetInteger("Facing", 0);
        anim.SetFloat("Speed", 0);
        lastDirection = Vector2.down;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed);
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector2.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            direction.y += 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction.x -= 1;
            sr.flipX = true;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction.y -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction.x += 1;
            sr.flipX = false;
        }
        if (direction != Vector2.zero)
        {
            direction = direction.normalized;
            lastDirection = direction;
            anim.SetFloat("Speed", 1f);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }

        anim.SetBool("IsHolding", playerInventory.HasItem);
        if (direction != Vector2.zero)
        {
            direction = direction.normalized;

            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                anim.SetInteger("Facing", direction.y > 0 ? 1 : 0);
            }
            else
            {
                anim.SetInteger("Facing", 2);
                sr.flipX = direction.x < 0;
            }

            lastDirection = direction;
        }
        else
        {
            if (Mathf.Abs(lastDirection.y) > Mathf.Abs(lastDirection.x))
            {
                anim.SetInteger("Facing", lastDirection.y > 0 ? 1 : 0);
            }
            else
            {
                anim.SetInteger("Facing", 2);
                sr.flipX = lastDirection.x < 0;
            }
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (playerInventory.CurrentItem != null && nearbyItem != null)
            {
                SwitchItems(playerInventory, nearbyItem);
            }
        }
    }

    public void SetNearbyItem(CollectibleItem item)
    {
        nearbyItem = item;
    }

    public void SwitchItems(PlayerInventory pi, CollectibleItem ci)
    {
        Vector2 spawnPosition = ci.transform.position;
        handItem = pi.CurrentItem;
        floorItem = ci.ItemData;
        pi.AddItem(floorItem);
        SpawnCollectibleAfterSwitch(handItem, spawnPosition);
        Destroy(ci.gameObject);
        nearbyItem = null;
    }

    public void SpawnCollectibleAfterSwitch(Item it, Vector2 pos)
    {
        GameObject switchObj = Instantiate(it.WorldPrefab, pos, Quaternion.identity);
        switchObj.GetComponent<CollectibleItem>().ItemData = it;
    }
}