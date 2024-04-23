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
}