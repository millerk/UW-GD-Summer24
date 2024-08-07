using UnityEngine;

public class CannonSearchingState : CannonBaseState
{
    public CannonSearchingState(CannonStateMachine currentContext, CannonStateFactory cannonStateFactory)
    : base (currentContext, cannonStateFactory){}

    public override void EnterState(){}

    public override void UpdateState()
    {
        CheckSwitchStates();

        // Set the cannon to point towards the front of the ship. 
        // Due to the way that movement is set up, angles have to be offset by 90 for the sprite to
        // travel in the direction we'd expect, otherwise we get a "car drifting" effect
        Rigidbody2D rb = _ctx.CannonRb;
        Rigidbody2D shipRb = _ctx.ShipRb;
        rb.rotation = shipRb.rotation - 90f;
    }

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        // Switch to lock on state if cannon has a target specified
        if (_ctx.Target != null)
        {
            SwitchState(_factory.LockedOn());
        }
    }
}
