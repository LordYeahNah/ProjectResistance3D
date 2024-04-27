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

    public override void OnEnter()
    {
        base.OnEnter();

        if(_stateMachine != null)
        {
            _stateMachine.SetStateProperty(StateMachineKeys.IS_AT_COVER_POSITION, false);
            _stateMachine.SetStateProperty(StateMachineKeys.HAS_COVER_POSITION, false);
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        if(_stateMachine.Ctrl != null)
        {
            var coverPointCtrl = _stateMachine.Ctrl.FindNearestCoverPoint();
            if(coverPointCtrl != null)
            {
                CoverPointInfo coverPointInfo;
                switch(coverPointCtrl.CoverType)
                {
                    case ECoverType.COVER_Side:
                        coverPointInfo = coverPointCtrl.GetEndPoint(_stateMachine.Ctrl.GlobalPosition);
                        break;
                    case ECoverType.COVER_Top:
                        coverPointInfo = coverPointCtrl.GetClosesCoverPointToSelf(_stateMachine.Ctrl.GlobalPosition);
                        break;
                    default:
                        coverPointInfo = coverPointCtrl.GetEndPoint(_stateMachine.Ctrl.GlobalPosition);
                        break;
                }

                _stateMachine.Ctrl.CurrentCoverPointInfo = coverPointInfo;

                _stateMachine.SetStateProperty<Vector3>(StateMachineKeys.MOVE_TO_LOCATION, coverPointInfo.GlobalPosition);
                _stateMachine.SetStateProperty<bool>(StateMachineKeys.HAS_MOVE_TO_LOCATION, true);
                _stateMachine.SetStateProperty<bool>(StateMachineKeys.HAS_COVER_POSITION, true);

                OnFinish();
            }
        }
    }
}