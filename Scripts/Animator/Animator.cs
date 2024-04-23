using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class Animator : StateMachine
{
    protected List<Animation> _animations = new List<Animation>();
    protected AnimationPlayer _animPlayer;

    public override void OnStart(CharacterController ctrl)
    {
        _animPlayer = ctrl.AnimPlayer;
        CreateAnimator();
        base.OnStart(ctrl);
        
    }

    public void PlayAnimation(string animName)
    {
        foreach (var anim in _animations)
        {
            if (anim.AnimationName == animName)
            {
                if (_animPlayer != null)
                    _animPlayer.Play(animName);
                return;
            }
        }
    }

    public void AddAnimation(Animation anim, bool entry = false)
    {
        _animations.Add(anim);
        if (entry)
            EntryState = anim;
    }

    protected virtual void CreateAnimator()
    {
        SetStateProperty("IsMoving", false, EPropertyType.PROP_Bool);
        
        var idleAnim = new Animation(this, "Idle_Unarmed_01", 8.35f, false, true);
        var walkAnim = new Animation(this, "Walk_Unarmed_01", 1.0333f, false, true);

        var idleToWalk = new StateTransition
        {
            NextState = walkAnim
        };
        idleToWalk._requiredProps.Add(new StateValue<bool>("IsMoving", true, EPropertyType.PROP_Bool));
        idleAnim.Transitions.Add(idleToWalk);

        var walkToIdle = new StateTransition()
        {
            NextState = idleAnim
        };
        walkToIdle._requiredProps.Add(new StateValue<bool>("IsMoving", false, EPropertyType.PROP_Bool));
        walkAnim.Transitions.Add(walkToIdle);
        
        AddAnimation(idleAnim, true);
        AddAnimation(walkAnim);
        
    }
}