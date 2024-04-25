using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class SightController : Node3D
{
    protected CharacterController _owner;                 // Store reference to the character controller

    [Export] protected float _coneAngle;
    [Export] protected float _sightDistance;
    public List<CharacterController> Enemies = new List<CharacterController>();
    protected List<CharacterController> _currentEnemiesInSight = new List<CharacterController>();

    public event Action<CharacterController> CharacterSeenEvent;
    public event Action<CharacterController> CharacterLostSightEvent;

    public override void _Ready()
    {
        base._Ready();
        _owner = GetParent<CharacterController>();
        if (_owner == null)
            GD.PrintErr("SightController -> Failed to get reference to the character owner");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        DetectSight();
    }

    protected void DetectSight()
    {
        foreach (var enemy in Enemies)
        {
            var toTarget = enemy.GlobalTransform.Origin - this.GlobalTransform.Origin;
            var forward = GlobalTransform.Basis.X;
            toTarget = toTarget.Normalized();
            forward = forward.Normalized();

            float dot = toTarget.Dot(forward);
            float angle = Mathf.RadToDeg(Mathf.Cos(dot));

            if (angle < _coneAngle / 2 && toTarget.Length() < _sightDistance)
            {
                if (CanSeeHead(enemy) || CanSeeBody(enemy) || CanSeeFeet(enemy))
                {
                    if (!_currentEnemiesInSight.Contains(enemy))
                    {
                        enemy.OnCharacterSeen(_owner);
                        _currentEnemiesInSight.Add(enemy);
                        CharacterSeenEvent?.Invoke(enemy);
                    }
                }
            } else
            {
                if(_currentEnemiesInSight.Contains(enemy))
                {
                    enemy.OnCharacterHidden(_owner);
                    _currentEnemiesInSight.Remove(enemy);
                    CharacterSeenEvent?.Invoke(enemy);
                }
            }
        }
    }

    private bool CanSeeHead(CharacterController enemy)
    {
        var fromPos = _owner.Sight.GlobalTransform.Origin;
        var toPos = enemy.HeadSight;

        var spaceState = GetWorld3D().DirectSpaceState;
        var rayParams = new PhysicsRayQueryParameters3D();
        rayParams.From = fromPos;
        rayParams.To = toPos;
        rayParams.CollideWithBodies = true;
        var result = spaceState.IntersectRay(rayParams);

        if(result.Count > 0)
        {
            if (((GodotObject)result["collider"]) is CharacterBody3D body)
            {
                if (body is CharacterController)
                    return true;
            }
            return true;
        }

        return false;
    }

    private bool CanSeeBody(CharacterController enemy)
    {
        var fromPos = _owner.Sight.GlobalTransform.Origin;
        var toPos = enemy.BodySight;

        var spaceState = GetWorld3D().DirectSpaceState;
        var rayParams = new PhysicsRayQueryParameters3D();
        rayParams.From = fromPos;
        rayParams.To = toPos;
        rayParams.CollideWithBodies = true;
        var result = spaceState.IntersectRay(rayParams);

        if (result.Count > 0)
        {
            if (((GodotObject)result["collider"]) is CharacterBody3D body)
            {
                if (body is CharacterController)
                    return true;
            }
            return true;
        }

        return false;
    }

    private bool CanSeeFeet(CharacterController enemy)
    {
        var fromPos = _owner.Sight.GlobalTransform.Origin;
        var toPos = enemy.FeetSight;

        var spaceState = GetWorld3D().DirectSpaceState;
        var rayParams = new PhysicsRayQueryParameters3D();
        rayParams.From = fromPos;
        rayParams.To = toPos;
        rayParams.CollideWithBodies = true;
        var result = spaceState.IntersectRay(rayParams);

        if (result.Count > 0)
        {
            if (((GodotObject)result["collider"]) is CharacterBody3D body)
            {
                if (body is CharacterController)
                    return true;
            }
            return true;
        }

        return false;
    }
}