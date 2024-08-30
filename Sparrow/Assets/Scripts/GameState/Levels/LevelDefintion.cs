using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level Configuration")]
public class LevelDefinition : ScriptableObject
{
    [System.Serializable]
    public class EnemyConfiguration
    {
        public GameObject Enemy;
        public int Count;
    }

    // mark as new  to tell Unity to always use this specific value when referencing name
    public new string name;
    public List<EnemyConfiguration> enemiesToSpawn = new List<EnemyConfiguration>();
    public LevelDefinition nextLevel;
}