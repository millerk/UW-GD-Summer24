using UnityEngine;

public class Gold : DroppableItem
{
    public int value;
    public ItemDef goldDef;
    
    public void OnEnable()
    {
        value = Random.Range(goldDef.minQuantity, goldDef.maxQuantity);
    }
}
