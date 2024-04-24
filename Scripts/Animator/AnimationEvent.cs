using System;
using System.Collections.Generic;
using Godot;

namespace NexusExtensions.Animation;

public class AnimationEvent
{
    public event Action AnimEvent;                      // reference to the events
    protected float _eventTime;                         // Time in the event the event 
    public float EventTime => _eventTime;               // Getter for the event time
    protected bool _hasFired;                               // Has the event occured
    public bool HasFired => _hasFired;                  

    public AnimationEvent(Action e, float time)
    {
        AnimEvent += e;
        _eventTime = time;
    }

    public void FireEvent()
    {
        AnimEvent?.Invoke();
        _hasFired = true;
    }

    public void OnFinish()
    {
        _hasFired = false;
    }
}