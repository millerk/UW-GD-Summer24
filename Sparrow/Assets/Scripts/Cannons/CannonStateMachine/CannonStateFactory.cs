public class CannonStateFactory
{
    CannonStateMachine _context;

    public CannonStateFactory(CannonStateMachine currentContext)
    {
        _context = currentContext;
    }

    public CannonBaseState Searching()
    {
        return new CannonSearchingState(_context, this);
    }
    public CannonBaseState LockedOn()
    {
        return new CannonLockedOnState(_context, this);
    }
    public CannonBaseState Firing()
    {
        return new CannonFiringState(_context, this);
    }
}
