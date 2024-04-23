using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class WanderSubState : SubStateMachine
{
    public WanderSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateStates();
    }

    private void CreateStates()
    {
        State FindLocation = new FindRandomLocation(_stateMachine, this, false, false);
        State moveToLocation = new MoveToLocationState(_stateMachine, this, false, false);
        State wait = new WaitState(_stateMachine, this, false, false);

        FindLocation.NextState = moveToLocation;
        moveToLocation.NextState = wait;
        wait.NextState = FindLocation;

        EntryState = FindLocation;
    }
}