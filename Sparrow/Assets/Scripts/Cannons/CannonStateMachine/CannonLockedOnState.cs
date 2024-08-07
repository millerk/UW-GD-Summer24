using UnityEngine;

public class CannonLockedOnState : CannonBaseState
{
    private float _distanceToTarget;
    public CannonLockedOnState(CannonStateMachine currentContext, CannonStateFactory cannonStateFactory)
    : base (currentContext, cannonStateFactory){}

    public override void EnterState()
    {
        UpdateTargetInfo();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        UpdateTargetInfo();
    }

    public override void ExitState()
    {
        _distanceToTarget = float.MaxValue;
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.Target is null)
        {
            SwitchState(_factory.Searching());
        }
        else if (_ctx.CanFire)
        {
            if (_distanceToTarget < _ctx.ProjectileConfig.maxDistance)
            {
                SwitchState(_factory.Firing());
            }
        }
    }

    void UpdateTargetInfo()
    {
        Rigidbody2D rb = _ctx.CannonRb;
        _distanceToTarget = Vector2.Distance(_ctx.Target.transform.position, _ctx.ProjectileSpawnPoint.position);
        
        // Have cannon track the target's location
        Vector2 dirToTarget = (Vector2)_ctx.Target.transform.position - rb.position;
        float angleToTarget = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg + 180;
        rb.rotation = angleToTarget;
    }
}
