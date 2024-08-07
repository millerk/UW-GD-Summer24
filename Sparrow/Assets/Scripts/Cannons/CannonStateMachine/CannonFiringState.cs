public class CannonFiringState : CannonBaseState
{
    public CannonFiringState(CannonStateMachine currentContext, CannonStateFactory cannonStateFactory)
    : base (currentContext, cannonStateFactory){}
    public override void EnterState()
    {
        _ctx.CannonAnim.SetTrigger("fire");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates()
    {
        if (!_ctx.CanFire)
        {
            SwitchState(_factory.LockedOn());
        }
    }
}
