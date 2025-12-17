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
    private CollectibleItem nearbyItem;
    [SerializeField] private float speed = 0.2f;
    public CollectibleItem NearbyItem => nearbyItem;

    // Referência para o slot da mão
    [SerializeField] private Transform handSlot;
    private SpriteRenderer handSpriteRenderer;

    // ADICIONE ESTE CAMPO para ajustar o tamanho
    [SerializeField] private float itemScale = 0.5f;

    Rigidbody2D rb;
    PlayerInventory playerInventory;
    Animator anim;
    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInventory = GetComponent<PlayerInventory>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Configuração do item na mão
        if (handSlot != null)
        {
            handSpriteRenderer = handSlot.GetComponent<SpriteRenderer>();
            if (handSpriteRenderer != null)
            {
                handSpriteRenderer.enabled = false; // Começa invisível
            }

            // Aplica o scale inicial
            ApplyItemScale();
        }
        else
        {
            Debug.LogWarning("HandSlot não atribuído ao PlayerController!");
        }

        lastDirection = Vector2.down;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed);
    }

    void Update()
    {
        HandleInput();
        HandleAnimation();
        HandleInteraction();
        UpdateHandItem(); // Atualiza item na mão
    }

    void HandleInput()
    {
        direction = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) direction.y += 1;
        if (Keyboard.current.sKey.isPressed) direction.y -= 1;
        if (Keyboard.current.aKey.isPressed) direction.x -= 1;
        if (Keyboard.current.dKey.isPressed) direction.x += 1;

        if (direction.magnitude > 0.1f)
        {
            Vector2 rawDirection = direction;
            direction.Normalize();
            lastDirection = rawDirection.normalized;
        }
    }

    void HandleAnimation()
    {
        bool isMoving = direction.magnitude > 0.1f;
        bool isHolding = playerInventory.HasItem;
        Vector2 currentDir = isMoving ? direction : lastDirection;

        string animationToPlay = GetAnimationName(isMoving, isHolding, currentDir);

        anim.CrossFade(animationToPlay, 0.1f);

        if (Mathf.Abs(currentDir.x) > Mathf.Abs(currentDir.y))
        {
            sr.flipX = currentDir.x < 0;
        }
    }

    // Método SIMPLIFICADO: Atualiza o item na mão
    void UpdateHandItem()
    {
        if (handSpriteRenderer == null) return;

        if (playerInventory.HasItem && playerInventory.CurrentItem != null)
        {
            // Mostra o sprite do item
            handSpriteRenderer.sprite = playerInventory.CurrentItem.Icon;
            handSpriteRenderer.enabled = true;

            // Aplica o scale atual
            ApplyItemScale();

            // Aplica flip se necessário
            ApplyHandFlip();
        }
        else
        {
            // Esconde o item
            handSpriteRenderer.enabled = false;
        }
    }

    // NOVO MÉTODO: Aplica o scale ao item
    void ApplyItemScale()
    {
        if (handSlot == null) return;

        // Aplica o scale uniforme (mesmo valor para X e Y)
        float currentScale = Mathf.Abs(handSlot.localScale.x); // Pega valor absoluto atual
        float targetScale = itemScale;

        // Se estiver flipado, mantém o negativo
        if (handSlot.localScale.x < 0)
        {
            handSlot.localScale = new Vector3(-targetScale, targetScale, 1f);
        }
        else
        {
            handSlot.localScale = new Vector3(targetScale, targetScale, 1f);
        }
    }

    // Aplica flip ao item se o personagem estiver virado
    void ApplyHandFlip()
    {
        if (handSlot == null) return;

        Vector2 currentDir = (direction.magnitude > 0.1f) ? direction : lastDirection;

        // Só aplica flip se for animação side e personagem estiver virado para esquerda
        string currentAnim = GetAnimationName(false, playerInventory.HasItem, currentDir);
        if (currentAnim.Contains("Side") && sr.flipX)
        {
            handSlot.localScale = new Vector3(-itemScale, itemScale, 1f);
        }
        else
        {
            handSlot.localScale = new Vector3(itemScale, itemScale, 1f);
        }
    }

    string GetAnimationName(bool isMoving, bool isHolding, Vector2 direction)
    {
        string baseName = "MaleCook_";
        string stateName = "";

        if (direction.magnitude < 0.1f)
        {
            direction = lastDirection;
        }

        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (Mathf.Abs(absX - absY) < 0.2f)
        {
            stateName = "IdleSide";
        }
        else if (absY > absX)
        {
            if (direction.y > 0)
                stateName = "IdleUp";
            else
                stateName = "IdleDown";
        }
        else
        {
            stateName = "IdleSide";
        }

        if (isHolding)
        {
            stateName += "Holding";
        }

        if (isMoving)
        {
            stateName = stateName.Replace("Idle", "Walk");
        }

        return baseName + stateName;
    }

    void HandleInteraction()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && playerInventory.CurrentItem != null && nearbyItem != null)
        {
            SwitchItems(playerInventory, nearbyItem);
        }
    }

    public void SetNearbyItem(CollectibleItem item)
    {
        nearbyItem = item;
    }

    public void SwitchItems(PlayerInventory pi, CollectibleItem ci)
    {
        Vector2 spawnPosition = ci.transform.position;
        Item handItem = pi.CurrentItem;
        Item floorItem = ci.ItemData;
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