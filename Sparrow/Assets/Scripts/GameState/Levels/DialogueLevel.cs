using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Level/Dialogue Level")]
public class DialogueLevelDefinition : LevelDefinition
{
    // mark as new  to tell Unity to always use this specific value when referencing name
    public String DialogueScene;
}
