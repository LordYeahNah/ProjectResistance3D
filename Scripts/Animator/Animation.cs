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
    
    // === Animation Events === //
    public List<AnimationEvent> AnimEvents = new List<AnimationEvent>();
    
    
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

        // Check if any events are due to be fired
        foreach (var e in AnimEvents)
        {
            if (_currentAnimTime > e.EventTime && !e.HasFired)
            {
                e.FireEvent();
            }
        }
        
        if (_currentAnimTime > _animationLength)
            OnFinish();
    }

    public override void OnFinish()
    {
        base.OnFinish();
    
        // reset the events so when fire them again
        foreach (var e in AnimEvents)
            e?.OnFinish();
        
        if (Loop)
            _currentAnimTime = 0f;
    }

    public override void OnExit()
    {
        _currentAnimTime = 0f;
    }
}