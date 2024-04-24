using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class PathPointReachedUpdater : StateUpdater
{
    public static readonly float PATH_ADJUSTMENT = 0.2f;
    public PathPointReachedUpdater(State stateRef, StateMachine stateMach) : base(stateRef, stateMach)
    {
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        Vector3 currentPos = _stateMachine.Ctrl.GlobalPosition;
        Vector3 moveToTarget = _stateMachine.GetStateProperty<Vector3>("MoveToLocation");
        float distance = currentPos.DistanceTo(moveToTarget);

        if (distance < MoveToLocationState.STOPPING_DISTANCE + PATH_ADJUSTMENT)
        {
            _stateMachine.SetStateProperty<bool>("HasReachedPathPoint", true);
        }
    }
}