using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class PartolSubState : SubStateMachine
{
    public PartolSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateStates();
    }

    private void CreateStates()
    {
        // Create the states
        var getPathPoint = new GetPathPoint(_stateMachine, this, false, true);
        var moveToLocation = new MoveToLocationState(_stateMachine, this, false, false);
        var waitTask = new WaitState(_stateMachine, this, false, false)
        {
            WaitTime = 5.0f,
            ResetMoveToLocation = true
        };

        getPathPoint.NextState = moveToLocation;
        waitTask.NextState = getPathPoint;

        // Transition move to location to waiting
        var moveToWait = new StateTransition()
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("HasReachedPathPoint", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("WaitAtPathEnd", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("IsAtPathEnd", true, EPropertyType.PROP_Bool)
            },
            NextState = waitTask
        };

        // Transition to the next path point
        var moveToFind = new StateTransition()
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("HasReachedPathPoint", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("IsAtPathEnd", false, EPropertyType.PROP_Bool)
            },
            NextState = getPathPoint
        };

        StateUpdater pathEndUpdater = new FindEndOfPath(moveToLocation, _stateMachine, getPathPoint);
        _stateMachine.AllStateUpdaters.Add(pathEndUpdater);

        StateUpdater checkHasReachedPathPoint = new PathPointReachedUpdater(moveToLocation, _stateMachine);
        _stateMachine.AllStateUpdaters.Add(checkHasReachedPathPoint);
        
        moveToLocation.Transitions.Add(moveToWait);
        moveToLocation.Transitions.Add(moveToFind);
        EntryState = getPathPoint;
    }
}