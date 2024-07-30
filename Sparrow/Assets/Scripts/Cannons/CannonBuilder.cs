using UnityEngine;

// Class that sits at top-level cannon template prefab and stores the cannon definition and
// references to individual components
public class CannonBuilder : MonoBehaviour
{
    public Cannon cannonDef;
    public GameObject cannon;
    public GameObject cannonBase;
    public GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer _cannonSprite = cannon.GetComponent<SpriteRenderer>();
        _cannonSprite.sortingOrder = 1;
        Transform _cannonTransform = cannon.GetComponent<Transform>();
        _cannonTransform.localPosition = cannonDef.cannonSpriteOffsetFromBase;
        
        SpriteRenderer _cannonBaseSprite = cannonBase.GetComponent<SpriteRenderer>();
        _cannonBaseSprite.sortingOrder = 0;
        _cannonBaseSprite.sprite = cannonDef.cannonBaseSprite;
        
        // Point the child cannon animator component to the controller defined in Assets/Animations
        Animator _cannonAnimator = cannon.GetComponent<Animator>();
        _cannonAnimator.runtimeAnimatorController = cannonDef.animationController;

        // Set the child cannon spawn point to correct offset to match the sprite
        Transform _fpTransform = firePoint.GetComponent<Transform>();
        _fpTransform.localPosition = cannonDef.firePointOffset;
    }
}
