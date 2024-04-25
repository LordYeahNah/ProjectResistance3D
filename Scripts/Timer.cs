using System;
using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class Timer
{
    public float TimerLength;
    private float _currentTime;
    public bool Loop;
    public event Action TimerCompleteEvent;
    public bool IsActive;

    public Timer(float length, bool loop, Action e, bool isActive = true)
    {
        TimerLength = length;
        Loop = loop;
        TimerCompleteEvent += e;
        IsActive = isActive;
    }

    public void OnUpdate(float dt)
    {
        if (!IsActive)
            return;

        _currentTime += 1 * dt;
        if (_currentTime > TimerLength)
            CompleteTimer();
    }

    private void CompleteTimer()
    {
        TimerCompleteEvent?.Invoke();
        IsActive = Loop;
        _currentTime = 0f;
    }

    public void ResetTimer()
    {
        _currentTime = 0f;
    }
}