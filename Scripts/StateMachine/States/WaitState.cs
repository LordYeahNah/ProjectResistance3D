using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class WaitState : State
{
    public float WaitTime = 3.0f;
    private float _currentWaitTime = 0f;
    public bool ResetMoveToLocation;
    
    public WaitState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public WaitState(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (ResetMoveToLocation)
        {
           if(_stateMachine.Ctrl != null)
               _stateMachine.Ctrl.SetMoveToLocation(Vector3.Zero);
        }
    }
    
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        _currentWaitTime += 1 * dt;
        if (_currentWaitTime > WaitTime)
            OnFinish();
    }

    public override void OnExit()
    {
        _currentWaitTime = 0f;
    }
}