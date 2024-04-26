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
        SetStateProperty("HasTarget", false, EPropertyType.PROP_Bool);
        SetStateProperty<Node3D>("Target", null, EPropertyType.PROP_Node2D);
        SetStateProperty<bool>("IsInCombat", false, EPropertyType.PROP_Bool);
        
        // Setup the state machine
        SubStateMachine patrolSubState = new PartolSubState(this, false, false);
        EntryState = patrolSubState;

        var combatState = new CombatSubState(this, false, false);

        var patrolToCombat = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("IsInCombat", true, EPropertyType.PROP_Bool)
            },
            NextState = combatState
        };
        patrolSubState.Transitions.Add(patrolToCombat);

        var combatToPatrol = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("HasTarget", false, EPropertyType.PROP_Bool)
            },
            NextState = patrolSubState
        };
        combatState.Transitions.Add(combatToPatrol);
    }
}