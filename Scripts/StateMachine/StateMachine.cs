using Godot;
using System;
using System.Collections.Generic;

namespace NexusExtensions;

public class StateMachine
{
    private CharacterController _ctrl;
    public CharacterController Ctrl => _ctrl; 
    private List<StateProperty> _properties = new List<StateProperty>();
    public List<StateProperty> Properties => _properties;
    private State _currentState;                    // Reference to the current state updating
    public State EntryState;                            // Which state will start the state machine

    public List<StateUpdater> AllStateUpdaters = new List<StateUpdater>();
    private List<StateUpdater> _activeStateUpdaters = new List<StateUpdater>();

    public virtual void OnStart(CharacterController ctrl)
    {
        _ctrl = ctrl;
        if(EntryState != null)
            SetState(EntryState, true);
    }

    public virtual void OnUpdate(float dt)
    {
        _currentState?.OnUpdate(dt);                    // Update the state
        
        CheckStateUpdaters();
        
        foreach(var updater in _activeStateUpdaters)
            if(updater != null)
                updater.OnUpdate((float)dt);

        // check if there is any possible transitions
        State state = CheckForTransition();
        if(state != null)
            SetState(state);
        
        
    }

    private State CheckForTransition()
    {
        foreach (var trans in _currentState.Transitions)
        {
            if (trans.CanTransition(_properties))
                return trans.NextState;
        }

        return null;
    }

    /// <summary>
    /// Checks if there should be 
    /// </summary>
    private void CheckStateUpdaters()
    {
        foreach (var state in AllStateUpdaters)
        {
            if (_activeStateUpdaters.Contains(state))
            {
                if (_currentState != state.StateRef)
                {
                    state.OnExit();
                    _activeStateUpdaters.Remove(state);
                }
            }
            else
            {
                if (state.StateRef == _currentState)
                {
                    state.OnEnter();
                    _activeStateUpdaters.Add(state);
                }
            }
        }
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
    
    public void SetStateProperty<T>(string name, T value, EPropertyType propType = EPropertyType.PROP_NONE)
    {
        foreach (var prop in _properties)
        {
            if (prop.PropertyName == name)
            {
                if (prop is StateValue<T> propValue)
                {
                    propValue.Value = value;
                    return;
                }
            }
        }
        
        _properties.Add(new StateValue<T>(name, value, propType));
    }

    public T GetStateProperty<T>(string name)
    {
        foreach (var prop in _properties)
        {
            if (prop.PropertyName == name)
            {
                if (prop is StateValue<T> propValue)
                    return propValue.Value;
            }
        }

        return default;
    }
}
