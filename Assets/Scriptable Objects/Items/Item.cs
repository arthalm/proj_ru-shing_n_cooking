using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    [SerializeField] private GameObject worldPrefab;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public string Description => description;
    public GameObject WorldPrefab => worldPrefab;
}