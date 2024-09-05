using UnityEngine;

public class LootManager : MonoBehaviour
{
    public ItemDef[] lootTable;
    // Simple implementation for now. Just listen to enemy death and drop constant amount of gold
    // on it's location.
    // Input: SpawnLoc, assumed to be transform of the enemy that just died and the location in
    // game where we want the loot to drop
    public void SpawnLoot(GameObject spawnLoc)
    {
        float drawn = Random.Range(0f, 100f);
        foreach (ItemDef item in lootTable)
        {
            if (drawn <= item.chance)
            {
                Instantiate(item.item, spawnLoc.transform.position, Quaternion.identity);
                return;
            }
        }

    }
}
