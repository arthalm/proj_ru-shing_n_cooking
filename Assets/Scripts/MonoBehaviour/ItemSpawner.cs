using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item[] spawnableItems;
    [SerializeField] private float levelTime;
    [SerializeField] private float initialInterval;
    [SerializeField] private float finalInterval;
    private float elapsedTime;
    private float currentInterval;
    private float spawnTimer;
    private Vector2 minArea;
    private Vector2 maxArea;
    BoxCollider2D bc2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
        elapsedTime = 0f;
        spawnTimer = 0f;
        currentInterval = initialInterval;
        minArea = bc2D.bounds.min;
        maxArea = bc2D.bounds.max;
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
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Item randomItem = spawnableItems[randomIndex];
            float randomPosX = Random.Range(minArea.x, maxArea.x);
            float randomPosY = Random.Range(minArea.y, maxArea.y);
            Vector2 spawnPos = new(randomPosX, randomPosY);
            GameObject go = Instantiate(randomItem.WorldPrefab, spawnPos, Quaternion.identity);
            if (go.TryGetComponent<CollectibleItem>(out var ci))
            {
                ci.ItemData = randomItem;
            }
            spawnTimer = 0f;
        }
    }
}