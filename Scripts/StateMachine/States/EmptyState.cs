using Godot;
using NexusExtensions;

public class EmptyState : State
{
    public EmptyState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public EmptyState(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }
}