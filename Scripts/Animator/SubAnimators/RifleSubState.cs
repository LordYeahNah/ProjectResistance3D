using System;
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

        var coverIdle = new Animation(_stateMachine, this, "Cover_Idle", 2.2833f, false, true);
        var coverEmergeLeft = new Animation(_stateMachine, this, "Cover_Emerge_Left", 1.1f, false, false);
        var coverEmergeRight = new Animation(_stateMachine, this, "Cover_Emerge_Right", 1.1f, false, false);


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

        var walkToCover = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>(GeneralAnimKeys.IS_IN_COVER, true, EPropertyType.PROP_Bool),
            },
            NextState = coverIdle
        };

        var idleToCover = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>(GeneralAnimKeys.IS_IN_COVER, true, EPropertyType.PROP_Bool)
            },
            NextState = coverIdle
        };
        
        idle.Transitions.Add(idleToWalk);
        idle.Transitions.Add(idleToCover);
        walk.Transitions.Add(walkToIdle);
        walk.Transitions.Add(walkToCover);
        
        EntryState = idle;
        Animations.Add(idle);
        Animations.Add(walk);
        Animations.Add(coverIdle);
        Animations.Add(coverEmergeRight);
        Animations.Add(coverEmergeLeft);
    }
}