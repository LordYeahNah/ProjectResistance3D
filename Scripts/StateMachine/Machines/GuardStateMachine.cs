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
        SetStateProperty(StateMachineKeys.MOVE_TO_LOCATION, Vector3.Zero, EPropertyType.PROP_Vector3);
        SetStateProperty(StateMachineKeys.HAS_REACHED_PATH_POINT, false, EPropertyType.PROP_Bool);
        SetStateProperty(StateMachineKeys.WAIT_AT_END_OF_PATH, true, EPropertyType.PROP_Bool);
        SetStateProperty(StateMachineKeys.IS_AT_PATH_END, false, EPropertyType.PROP_Bool);
        SetStateProperty(StateMachineKeys.HAS_REACHED_PATH_POINT, false, EPropertyType.PROP_Bool);
        SetStateProperty(StateMachineKeys.HAS_TARGET, false, EPropertyType.PROP_Bool);
        SetStateProperty<Node3D>(StateMachineKeys.TARGET, null, EPropertyType.PROP_Node2D);
        SetStateProperty<bool>(StateMachineKeys.IS_IN_COMBAT, false, EPropertyType.PROP_Bool);
        SetStateProperty<bool>(StateMachineKeys.HAS_COVER_POSITION, false, EPropertyType.PROP_Bool);
        SetStateProperty<bool>(StateMachineKeys.IS_AT_COVER_POSITION, false, EPropertyType.PROP_Bool);
        SetStateProperty<bool>(StateMachineKeys.WILL_SHOOT, false, EPropertyType.PROP_Bool);
        SetStateProperty<bool>(StateMachineKeys.SHOOT_NOW, false, EPropertyType.PROP_Bool);


        // Setup the state machine
        SubStateMachine patrolSubState = new PartolSubState(this, false, false);
        EntryState = patrolSubState;

        var combatState = new CombatSubState(this, false, false);

        var patrolToCombat = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>(StateMachineKeys.IS_IN_COMBAT, true, EPropertyType.PROP_Bool)
            },
            NextState = combatState
        };
        patrolSubState.Transitions.Add(patrolToCombat);

        var combatToPatrol = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>(StateMachineKeys.HAS_TARGET, false, EPropertyType.PROP_Bool)
            },
            NextState = patrolSubState
        };
        combatState.Transitions.Add(combatToPatrol);
    }
}