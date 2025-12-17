using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private Item[] itemList;
    [SerializeField] private Item finalItem;
    [SerializeField] private float time;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;

    public Item[] ItemList => itemList;
    public Item FinalItem => finalItem;
    public float Time => time;
    public Sprite Icon => icon;
    public string Description => description;
}