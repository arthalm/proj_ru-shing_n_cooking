using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item[] spawnableItems;
    [SerializeField] private float levelTime;
    [SerializeField] private float initialInterval;
    [SerializeField] private float finalInterval;
    [SerializeField] private float minDistanceBetweenItems = 1.5f;
    private float elapsedTime;
    private float currentInterval;
    private float spawnTimer;
    private Vector2 minArea;
    private Vector2 maxArea;
    private List<Vector2> spawnedPositions = new();
    PolygonCollider2D pc2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pc2D = GetComponent<PolygonCollider2D>();
        elapsedTime = 0f;
        spawnTimer = 0f;
        currentInterval = initialInterval;
        minArea = pc2D.bounds.min;
        maxArea = pc2D.bounds.max;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / levelTime);
        currentInterval = Mathf.Lerp(initialInterval, finalInterval, t);

        if (spawnTimer >= currentInterval)
        {
            TrySpawnItem();
            spawnTimer = 0f;
        }

        if (Time.frameCount % 300 == 0)
        {
            ClearOldPositions();
        }
    }

    void TrySpawnItem()
    {
        int randomIndex = Random.Range(0, spawnableItems.Length);
        Item randomItem = spawnableItems[randomIndex];

        Vector2 spawnPos = FindValidSpawnPosition();

        if (spawnPos != Vector2.negativeInfinity)
        {
            GameObject go = Instantiate(randomItem.WorldPrefab, spawnPos, Quaternion.identity);
            if (go.TryGetComponent<CollectibleItem>(out var ci))
            {
                ci.ItemData = randomItem;
            }

            spawnedPositions.Add(spawnPos);
        }
        else
        {
            Debug.Log("Não foi possível encontrar posição para spawnar item!");
        }
    }

    Vector2 FindValidSpawnPosition(int maxAttempts = 30)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPos = GetRandomPosition();

            if (IsPositionValid(randomPos))
            {
                return randomPos;
            }
        }

        return Vector2.negativeInfinity;
    }

    Vector2 GetRandomPosition()
    {
        float randomPosX = Random.Range(minArea.x, maxArea.x);
        float randomPosY = Random.Range(minArea.y, maxArea.y);
        return new Vector2(randomPosX, randomPosY);
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 spawnedPos in spawnedPositions)
        {
            float distance = Vector2.Distance(position, spawnedPos);
            if (distance < minDistanceBetweenItems)
            {
                return false;
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, minDistanceBetweenItems / 2f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && !collider.isTrigger)
            {
                if (collider.GetComponent<CollectibleItem>() != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void ClearOldPositions()
    {
        if (spawnedPositions.Count > 20)
        {
            spawnedPositions.RemoveRange(0, spawnedPositions.Count - 20);
        }
    }
}