using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class SubStateMachine : State
{
    protected State _currentState;
    public State EntryState;
    
    public SubStateMachine(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(EntryState != null)
            SetState(EntryState, true);
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (_currentState != null)
            _currentState.OnUpdate(dt);

        State nextState = CheckForTransition();
        if(nextState != null)
            SetState(nextState);
    }
    
    private State CheckForTransition()
    {
        foreach (var trans in _currentState.Transitions)
        {
            if (trans.CanTransition(_stateMachine.Properties))
                return trans.NextState;
        }

        return null;
    }
    
    /// <summary>
    /// Sets the new state to transition to
    /// </summary>
    /// <param name="state">Reference to the state to transition to</param>
    /// <param name="confirmedExit">If the state is ready to exit</param>
    public void SetState(State state, bool confirmedExit = false)
    {
        if (_currentState != null)
        {
            if (_currentState.HasExit)
            {
                if (confirmedExit)
                {
                    _currentState.OnExit();
                    _currentState = state;
                    if(_currentState != null)
                        _currentState.OnEnter();
                }
                else
                {
                    _currentState.NextState = state;
                    _currentState.ResetState = true;
                }
            }
            else
            {
                _currentState.OnExit();
                _currentState = state;
                if(_currentState != null)
                    _currentState.OnEnter();
            }
        }
        else
        {
            _currentState = state;
            if(_currentState != null)
                _currentState.OnEnter();
        }
    }
}