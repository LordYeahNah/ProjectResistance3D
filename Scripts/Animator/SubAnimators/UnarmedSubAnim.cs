using System.Collections.Generic;
using Godot;
using NexusExtensions;
using NexusExtensions.Animation;
using Animation = NexusExtensions.Animation.Animation;

public class UnarmedSubAnim : AnimatorSubState
{
    public UnarmedSubAnim(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateAnimator();
    }

    private void CreateAnimator()
    {
        var idleAnim = new Animation(_stateMachine, this, "Idle_Unarmed_01", 8.35f, false, true);
        var walkAnim = new Animation(_stateMachine, this, "Walk_Unarmed_01", 1.0333f, false, true);

        var idleToWalk = new StateTransition
        {
            NextState = walkAnim
        };
        idleToWalk._requiredProps.Add(new StateValue<bool>("IsMoving", true, EPropertyType.PROP_Bool));
        idleAnim.Transitions.Add(idleToWalk);

        var walkToIdle = new StateTransition
        {
            NextState = idleAnim
        };
        walkToIdle._requiredProps.Add(new StateValue<bool>("IsMoving", false, EPropertyType.PROP_Bool));
        walkAnim.Transitions.Add(walkToIdle);

        var testEvent = new AnimationEvent(TestAnimEvent, 4.3f);
        idleAnim.AnimEvents.Add(testEvent);
        
        EntryState = idleAnim;
        Animations.Add(idleAnim);
        Animations.Add(walkAnim);
    }

    private void TestAnimEvent()
    {
        GD.Print("Event Fired");
    }
}