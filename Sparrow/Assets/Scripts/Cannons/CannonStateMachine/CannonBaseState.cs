public abstract class CannonBaseState
{
    protected CannonStateMachine _ctx;
    protected CannonStateFactory _factory;
    public CannonBaseState(CannonStateMachine currentContext, CannonStateFactory cannonStateFactory)
    {
        _ctx = currentContext;
        _factory = cannonStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    protected void SwitchState(CannonBaseState newState){
        ExitState();
        newState.EnterState();
        _ctx.CurrentState = newState;
    }
}
