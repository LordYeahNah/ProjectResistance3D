using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class WeaponCooldown : StateUpdater
{
    public WeaponCooldown(State stateRef, StateMachine stateMach) : base(stateRef, stateMach)
    {
    }
}