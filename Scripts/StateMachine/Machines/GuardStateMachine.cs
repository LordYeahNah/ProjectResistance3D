using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class GuardStateMachine : StateMachine
{
    public GuardStateMachine()
    {
        CreateStateMachine();
    }

    private void CreateStateMachine()
    {
        
        // Create the properties
        SetStateProperty("MoveToLocation", Vector3.Zero, EPropertyType.PROP_Vector3);
        SetStateProperty("HasMoveToLocation", false, EPropertyType.PROP_Bool);
        SetStateProperty("WaitAtPathEnd", true, EPropertyType.PROP_Bool);
        SetStateProperty("IsAtPathEnd", false, EPropertyType.PROP_Bool);
        SetStateProperty("HasReachedPathPoint", false, EPropertyType.PROP_Bool);
        
        // Setup the state machine
        SubStateMachine patrolSubState = new PartolSubState(this, false, false);
        EntryState = patrolSubState;
    }
}