using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHealth : DroppableItem
{
    public int value;

    public ItemDef soulHealthDef;

    void OnEnable()
    {
        value = Random.Range(soulHealthDef.minQuantity, soulHealthDef.maxQuantity);
    }
}
