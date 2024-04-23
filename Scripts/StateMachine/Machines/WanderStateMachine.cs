using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class WanderStateMachine : StateMachine
{
    public override void OnStart(CharacterController ctrl)
    {
        CreateStateMachine();
        base.OnStart(ctrl);
    }
    public void CreateStateMachine()
    {
        SubStateMachine partolSubState = new WanderSubState(this, false, false);
        EntryState = partolSubState;
    }
}