using System.Collections.Generic;
using Godot;
using NexusExtensions;
using NexusExtensions.Animation;
using Animation = NexusExtensions.Animation.Animation;

public class RifleSubState : AnimatorSubState
{
    public RifleSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateAnimations();
    }

    private void CreateAnimations()
    {
        var idle = new Animation(_stateMachine, this, "Idle_Rifle_01", 10.65f, false, true);
        var walk = new Animation(_stateMachine, this, "Walk_Rifle_01", 1.0167f, false, true);

        var idleToWalk = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("IsMoving", true, EPropertyType.PROP_Bool),
            },
            NextState = walk
        };

        var walkToIdle = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("IsMoving", false, EPropertyType.PROP_Bool)
            },
            NextState = idle
        };
        
        idle.Transitions.Add(idleToWalk);
        walk.Transitions.Add(walkToIdle);
        
        EntryState = idle;
        Animations.Add(idle);
        Animations.Add(walk);
    }
}