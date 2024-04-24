using System.Collections.Generic;
using Godot;

namespace NexusExtensions.Animation;

public class Animation : State
{
    // === Animation details === //
    protected Animator _animOwner;
    protected string _animationName;
    protected float _animationLength;

    public string AnimationName => _animationName;
    
    // === Animation Updates === //
    protected float _currentAnimTime;
    
    
    public Animation(StateMachine stateMach, string animName, float length, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        if (_stateMachine is Animator)
        {
            _animOwner = (Animator)_stateMachine;
        }

        _animationName = animName;
        _animationLength = length;
    }

    public Animation(StateMachine stateMach, SubStateMachine subState, string animName, float length, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
        if (_stateMachine is Animator)
        {
            _animOwner = (Animator)_stateMachine;
        }

        _animationName = animName;
        _animationLength = length;
    }

    public override void OnEnter()
    {
        if (IS_OWNED_BY_SUBSTATE)
        {
            if (_subState is AnimatorSubState subState)
            {
                subState.PlayAnimation(AnimationName);
            }
        }
        else
        {
            _animOwner?.PlayAnimation(AnimationName);
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        _currentAnimTime += 1 * dt;
        if (_currentAnimTime > _animationLength)
            OnFinish();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        
        if (Loop)
            _currentAnimTime = 0f;
    }

    public override void OnExit()
    {
        _currentAnimTime = 0f;
    }
}