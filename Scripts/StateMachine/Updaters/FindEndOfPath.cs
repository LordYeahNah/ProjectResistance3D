using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class FindEndOfPath : StateUpdater
{
    private GetPathPoint _pathPointState;
    private PathPointController _pathPoint;
    public FindEndOfPath(State stateRef, StateMachine stateMach, GetPathPoint pathPointCtrl) : base(stateRef, stateMach)
    {
        _pathPointState = pathPointCtrl;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (_pathPointState != null)
        {
            if (_pathPoint == null)
            {
                _pathPoint = _pathPointState.PathPointCtrl;
            }
        }
        else
        {
            GD.PrintErr("FindEndOfPath -> Reference to the path point state null");
        }
        
        if(_pathPoint == null)
            GD.PrintErr("FindEndOfPath -> Failed to get reference to the path point controller");
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        if (_pathPoint != null)
        {
            int pathPointCount = _pathPoint.PathCount;
            if (_pathPoint.CirclePath)
            {
                if (_pathPointState != null)
                {
                    if (_pathPointState.CurrentPathIndex == (pathPointCount - 1))
                    {
                        _stateMachine.SetStateProperty("IsAtPathEnd", true, EPropertyType.PROP_Bool);
                    }
                }
            }
            else
            {
                if (_pathPointState != null)
                {
                    if (_pathPointState.CurrentPathIndex >= (pathPointCount - 1) || _pathPointState.CurrentPathIndex <= 0)
                    {
                        _stateMachine.SetStateProperty("IsAtPathEnd", true, EPropertyType.PROP_Bool);
                    }
                }
            }
        }
    }
}