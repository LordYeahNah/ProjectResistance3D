using System.Collections.Generic;
using Godot;
using NexusExtensions;
using NexusExtensions.Animation;
using Animation = NexusExtensions.Animation.Animation;

public class AnimatorSubState : SubStateMachine
{
    public List<Animation> Animations = new List<Animation>();
    protected Animation _currentAnimation;
    public Animation EntryAnim;
    protected AnimationPlayer _animPlayer;
    
    public AnimatorSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        
        if (_animPlayer == null)
        {
            if (_stateMachine != null && _stateMachine is Animator anim)
            {
                _animPlayer = anim.AnimPlayer;
            }
        }
        
        if(_animPlayer == null)
            GD.PrintErr("Failed to get reference to animator");
        
        base.OnEnter();
    }

    public virtual void PlayAnimation(string animName)
    {
        foreach (var anim in Animations)
        {
            if (anim != null && anim.AnimationName == animName)
            {
                if(_animPlayer != null)
                    _animPlayer.Play(animName);
                return;
            }
        }
    }
}