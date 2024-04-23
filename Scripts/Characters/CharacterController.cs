using System.Collections.Generic;
using Godot;
using NexusExtensions;

public partial class CharacterController : CharacterBody3D
{
    // === Movement Settings === //
    [Export] private float _partolMovementSpeed;
    [Export] private float _generalMovementSpeed;
    
    // === Rotation Settings === //
    [Export] private float _rotationSpeed;
    
    // === Animation settings === //
    protected AnimationPlayer _animPlayer;
    public AnimationPlayer AnimPlayer => _animPlayer;
    protected Animator _anim;                   // Reference to the animator

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
    // == AI == //
    private NavigationAgent3D _agent;
    public NavigationAgent3D Agent => _agent;
    private StateMachine _stateMachine;
    public StateMachine StateMachineRef => _stateMachine;
    
    // === Follow Path Settings === //
    [ExportGroup("Follow Path Settings")]
    [Export] private PathPointController _followPathController;                         // reference to the path the character is following
    public PathPointController FollowPathController => _followPathController;
    

    public override void _Ready()
    {
        base._Ready();
        _agent = GetNode<NavigationAgent3D>("NavAgent");
        if(_agent == null)
            GD.PrintErr("CharacterController -> Failed to get reference to the navigation agent");

        _animPlayer = GetNode<AnimationPlayer>("Character/AnimationPlayer");
        if(_animPlayer == null)
            GD.PrintErr("CharacterController -> Failed to get reference to the animation player");

        _anim = new Animator();
        _anim.OnStart(this);

        Callable.From(ActorSetup).CallDeferred();
    }

    public override void _Process(double dt)
    {
        base._Process(dt);
        if(_stateMachine != null)
            _stateMachine.OnUpdate((float)dt);
        
        if(_anim != null)
            _anim.OnUpdate((float)dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        HandleMovement((float)delta);
        
    }

    private void HandleMovement(float dt)
    {
        if (!_hasMoveToLocation || _agent == null || _agent.IsNavigationFinished())
            return;

        var currentPos = GlobalTransform.Origin;
        var nextPathPosition = _agent.GetNextPathPosition();

        Velocity = currentPos.DirectionTo(nextPathPosition) * _generalMovementSpeed;
        MoveAndSlide();

        if (Velocity.Length() > 0f)
        {
            var angle = Mathf.Atan2(-Velocity.Z, Velocity.X);
            var targetRotation = new Quaternion(Vector3.Up, angle).Normalized();
            this.Rotation = Quaternion.Slerp(targetRotation, dt * _rotationSpeed).GetEuler();
        }
    }

    /// <summary>
    /// Sets the location for an agent to move and stops it from moving
    /// </summary>
    /// <param name="moveTo">Location to move to (set to Zero to stop)</param>
    public void SetMoveToLocation(Vector3 moveTo)
    {
        MoveToLocation = moveTo;
        _hasMoveToLocation = moveTo != Vector3.Zero;
        // Setup the animator
        if(_anim != null)
            _anim.SetStateProperty("IsMoving", _hasMoveToLocation);
    }

    public async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        CreateStateMachine();
    }

    protected virtual void CreateStateMachine()
    {
        _stateMachine = new WanderStateMachine();
        _stateMachine.OnStart(this);
    }
}