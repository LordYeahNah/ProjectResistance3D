using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class State
{

    protected StateMachine _stateMachine;                       // Store reference to the state machine
    public State NextState;                                 // State to transition to

    public string StateName;
    public string StateDescription;

    public List<StateTransition> Transitions = new List<StateTransition>();             // Store reference to the state transitions

    public bool HasExit;
    public bool ResetState = false;                         // Remove the next state when finished
    public bool Loop;

    protected readonly bool IS_OWNED_BY_SUBSTATE;
    protected SubStateMachine _subState;
    
    public State(StateMachine stateMach, bool hasExit = false, bool loop = false)
    {
        _stateMachine = stateMach;
        HasExit = hasExit;
        Loop = loop;
        IS_OWNED_BY_SUBSTATE = false;
    }

    public State(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false)
    {
        _stateMachine = stateMach;
        _subState = subState;
        HasExit = hasExit;
        Loop = loop;
        IS_OWNED_BY_SUBSTATE = true;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnUpdate(float dt)
    {
        
    }

    public virtual void OnFinish()
    {
        if (NextState != null)
        {
            if (IS_OWNED_BY_SUBSTATE)
            {
                _subState.SetState(NextState, true);
            }
            else
            {
                _stateMachine.SetState(NextState, true);
            }
            if (ResetState)
                NextState = null;
        }
    }

    public virtual void OnExit()
    {
        
    }
}