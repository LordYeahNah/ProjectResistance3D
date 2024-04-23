using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using NexusExtensions;

public class TestSubState : SubStateMachine
{
    public TestSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateSubState();
    }

    private void CreateSubState()
    {
        
    }
}