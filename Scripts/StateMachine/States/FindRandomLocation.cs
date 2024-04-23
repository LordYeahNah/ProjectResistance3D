using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class FindRandomLocation : State
{
    private NavigationAgent3D _agent;
    public FindRandomLocation(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public FindRandomLocation(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_agent == null && _stateMachine != null)
        {
            if (_stateMachine.Ctrl != null)
                _agent = _stateMachine.Ctrl.Agent;
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (_agent != null)
        {
            Vector3 location = NavigationServer3D.MapGetRandomPoint(_agent.GetNavigationMap(), 1, true);
            if (location != Vector3.Zero)
            {
                _stateMachine.SetStateProperty("MoveToLocation", location);
                _stateMachine.SetStateProperty("HasMoveToLocation", true);
            }
        }

        OnFinish();
    }
}