using System.Collections.Generic;
using Godot;
using NexusExtensions;

public partial class CharacterController : CharacterBody3D
{
    // === Movement Settings === //
    [Export]
    private float _partolMovementSpeed;
    [Export]
    private float _generalMovementSpeed;

    [Export]
    private Node3D _debugPosition;

    private bool _hasMoveToLocation = false;
    private Vector3 _moveToLocation;

    public Vector3 MoveToLocation
    {
        get => _moveToLocation;
        set
        {
            if (value != Vector3.Zero)
            {
                _moveToLocation = value;
                if (_agent != null)
                    _agent.TargetPosition = _moveToLocation;
            }
        }
    }
    
    // === Components === //
    private NavigationAgent3D _agent;
    

    public override void _Ready()
    {
        base._Ready();
        _agent = GetNode<NavigationAgent3D>("NavAgent");
        if(_agent == null)
            GD.PrintErr("CharacterController -> Failed to get reference to the navigation agent");
        
        Callable.From(ActorSetup).CallDeferred();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        HandleMovement();
        
    }

    private void HandleMovement()
    {
        if (!_hasMoveToLocation || _agent == null || _agent.IsNavigationFinished())
            return;

        Vector3 currentPos = GlobalTransform.Origin;
        Vector3 nextPathPosition = _agent.GetNextPathPosition() * _generalMovementSpeed;

        Velocity = currentPos.DirectionTo(nextPathPosition);
        MoveAndSlide();
    }

    /// <summary>
    /// Sets the location for an agent to move and stops it from moving
    /// </summary>
    /// <param name="moveTo">Location to move to (set to Zero to stop)</param>
    public void SetMoveToLocation(Vector3 moveTo)
    {
        MoveToLocation = moveTo;
        _hasMoveToLocation = moveTo != Vector3.Zero;
    }

    public async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        SetMoveToLocation(_debugPosition.Position);
    }
}