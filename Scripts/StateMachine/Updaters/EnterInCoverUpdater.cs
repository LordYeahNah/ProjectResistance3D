using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class EnterInCoverUpdater : State
{
    public EnterInCoverUpdater(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public EnterInCoverUpdater(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(_stateMachine != null)
        {
            if(!_stateMachine.GetStateProperty<bool>(StateMachineKeys.IS_AT_COVER_POSITION))
            {
                _stateMachine.SetStateProperty<bool>(StateMachineKeys.IS_AT_COVER_POSITION, true);
                if (_stateMachine.Ctrl != null)
                    _stateMachine.Ctrl.SetInCover(true);
            }
        }
    }
}