using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class CombatSubState : SubStateMachine
{
    public CombatSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    private void CreateStates()
    {

    }
}