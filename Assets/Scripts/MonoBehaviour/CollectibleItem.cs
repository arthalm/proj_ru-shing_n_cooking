using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private Item itemData;
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
        Debug.Log("o que deu trigger: " + other.name);
    }
}