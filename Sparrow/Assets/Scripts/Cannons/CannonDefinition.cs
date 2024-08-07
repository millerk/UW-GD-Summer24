using UnityEngine;

// ScriptableObject definition for a specific cannon configuration. 
// Used to instantiate a cannon from CannonTemplate
[CreateAssetMenu(fileName = "New Cannon", menuName ="Units/Cannon")]
public class Cannon : ScriptableObject
{
    // mark as new  to tell Unity to always use this specific value when referencing name
    public new string name;
    public string description;
    public int attackStrength;
    public float attackDistance;
    public Vector2 cannonSpriteOffsetFromBase;
    public Vector2 firePointOffset;
    public Sprite cannonBaseSprite;
    public GameObject projectile;
    public float projectileSpeed;
    public RuntimeAnimatorController animationController;
}
