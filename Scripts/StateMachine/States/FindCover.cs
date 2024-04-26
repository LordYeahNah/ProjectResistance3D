using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using NexusExtensions;

public class FindCover : State
{
    public FindCover(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public FindCover(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        if(_stateMachine.Ctrl != null)
        {
            var coverPointCtrl = _stateMachine.Ctrl.FindNearestCoverPoint();
            if(coverPointCtrl != null)
            {
                _stateMachine.SetStateProperty<Vector3>("MoveToLocation", coverPointCtrl.GetClosesCoverPointToSelf(_stateMachine.Ctrl.GlobalPosition));
                _stateMachine.SetStateProperty<bool>("HasMoveToLocation", true);

                OnFinish();
            }
        }
    }
}