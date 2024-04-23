using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class GetPathPoint : State
{
    private readonly float STOPPING_DISTANCE = 3f;
    private PathPointController _pathPointCtrl;                     // Store reference to the path point controller
    private int _currentPathIndex;
    
    private enum ECurrentDirection
    {
        DIR_Forwards,
        DIR_Backwards
    }

    private ECurrentDirection _currentDirection = ECurrentDirection.DIR_Forwards;
    
    public GetPathPoint(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public GetPathPoint(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public void OnEnter()
    {
        if (_pathPointCtrl == null)
        {
            _pathPointCtrl = _stateMachine.Ctrl.FollowPathController;
            if (_pathPointCtrl == null)
                GD.PrintErr("GetPathPoint -> Failed to get reference to the follow path");
        }
        base.OnEnter();
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (_pathPointCtrl == null)
            return;

        IncrementPath();                        // Get the nex tpath point
        
        // Get the reference from the controller
        Node3D nextPathPoint = _pathPointCtrl.GetPathPoint(_currentPathIndex);
        if (nextPathPoint != null)
        {
            // Update the properties of the state machine
            _stateMachine.SetStateProperty("MoveToLocation", nextPathPoint.GlobalPosition);
            _stateMachine.SetStateProperty("HasMoveToLocation", nextPathPoint.GlobalPosition);
        }

        OnFinish();                 // Finish the task
    }

    private void IncrementPath()
    {
        
        if (_currentDirection == ECurrentDirection.DIR_Forwards)
        {
            _currentPathIndex += 1;
            if (_currentPathIndex >= _pathPointCtrl.PathCount)
            {
                if (_pathPointCtrl.CirclePath)
                {
                    _currentPathIndex = 0;
                }
                else
                {
                    _currentDirection = ECurrentDirection.DIR_Backwards;
                    _currentPathIndex -= 1;
                }
            }
        }
        else
        {
            _currentPathIndex -= 1;
            if (_currentPathIndex < 0)
            {
                _currentDirection = ECurrentDirection.DIR_Forwards;
                _currentPathIndex += 1;
            }
        }
    }
}