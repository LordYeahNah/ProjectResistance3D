using System.Collections.Generic;
using Godot;

namespace NexusExtensions.Animation;

public class AnimatorSubState : SubStateMachine
{
    public AnimatorSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }
}