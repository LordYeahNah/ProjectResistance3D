using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class StateUpdater
{
    protected State _stateRef;
    public State StateRef => _stateRef;
    protected StateMachine _stateMachine;

    public StateUpdater(State stateRef, StateMachine stateMach)
    {
        _stateMachine = stateMach;
        _stateRef = stateRef;
    }
    
    public virtual void OnEnter()
    {
        
    }

    public virtual void OnUpdate(float dt)
    {
        
    }

    public virtual void OnExit()
    {
        
    }
}