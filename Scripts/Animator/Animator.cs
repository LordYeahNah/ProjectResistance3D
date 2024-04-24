using System.Collections.Generic;
using Godot;
using NexusExtensions;

namespace NexusExtensions.Animation;

public class Animator : StateMachine
{
    protected List<Animation> _animations = new List<Animation>();
    protected AnimationPlayer _animPlayer;
    public AnimationPlayer AnimPlayer => _animPlayer;

    public override void OnStart(CharacterController ctrl)
    {
        _animPlayer = ctrl.AnimPlayer;
        base.OnStart(ctrl);
    }

    public void PlayAnimation(string animName)
    {
        // Check within this animator for the animation to play
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
}