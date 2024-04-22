using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using NexusExtensions;

public class TestSubState : SubStateMachine
{
    public TestSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateSubState();
    }

    private void CreateSubState()
    {
        State empty = new EmptyState(_stateMachine, this, false, false);
        State moveTo = new MoveToLocationState(_stateMachine, this, false, false);

        StateTransition emptyToMove = new StateTransition();
        emptyToMove.NextState = moveTo;
        emptyToMove._requiredProps.Add(new StateValue<bool>("HasMoveToLocation", true, EPropertyType.PROP_Bool));
        empty.Transitions.Add(emptyToMove);

        StateTransition moveToEmpty = new StateTransition();
        moveToEmpty.NextState = empty;
        moveToEmpty._requiredProps.Add(new StateValue<bool>("HasMoveToLocation", false, EPropertyType.PROP_Bool));
        moveTo.Transitions.Add(moveToEmpty);

        EntryState = empty;
    }
}