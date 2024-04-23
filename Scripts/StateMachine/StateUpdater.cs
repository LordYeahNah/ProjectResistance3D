using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class StateUpdater
{
    private State _stateRef;
    public State StateRef => _stateRef;

    public StateUpdater(State stateRef)
    {
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