using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class MoveToLocationState : State
{
    public static readonly float STOPPING_DISTANCE = 1f;
    
    private CharacterController _ctrl;
    public MoveToLocationState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        StateName = "Move To Location";
        StateDescription = "Handles moving the character to the specified position";
    }

    public MoveToLocationState(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (_ctrl == null)
            _ctrl = _stateMachine.Ctrl;
        
        if(_ctrl == null)
            GD.PrintErr("MoveToLocation -> Failed to get reference to the controller");
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (_ctrl != null)
        {
            // Get the position properties
            var moveToPos = _stateMachine.GetStateProperty<Vector3>("MoveToLocation");
            var currentPos = _ctrl.GlobalPosition;
            var distance = currentPos.DistanceTo(moveToPos);
            if (distance < STOPPING_DISTANCE)
            {
                _ctrl.SetMoveToLocation(Vector3.Zero);
                _stateMachine.SetStateProperty("MoveToLocation", Vector3.Zero);
                _stateMachine.SetStateProperty("HasMoveToLocation", false);
                OnFinish();
            }
            else
            {
                _ctrl.SetMoveToLocation(moveToPos);
            }
        }
    }
}