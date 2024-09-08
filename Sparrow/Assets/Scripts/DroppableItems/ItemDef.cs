using UnityEngine;

[CreateAssetMenu(menuName = "Items/LootableItem")]
public class ItemDef : ScriptableObject
{
    public GameObject item;
    public float chance;
    public int minQuantity;
    public int maxQuantity;
}
