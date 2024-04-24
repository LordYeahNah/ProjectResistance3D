using Godot;
using NexusExtensions;
using NexusExtensions.Animation;

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
    
    
    // === Movement Properties === //
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
    [Export] private bool _followPathPoint;                             // Flag if the character is to follow a path point
    [Export] private PathPointController _followPathController;                         // reference to the path the character is following

    public PathPointController FollowPathController
    {
        get => _followPathController;
        set
        {
            _followPathController = value;
            if (_followPathController == null)
            {
                StopFollowPath();
            }
        }
    } 
    

    public override void _Ready()
    {
        base._Ready();
        // Get reference to the AI agent
        _agent = GetNode<NavigationAgent3D>("NavAgent");
        if(_agent == null)
            GD.PrintErr("CharacterController -> Failed to get reference to the navigation agent");

        // Get reference to the animation player
        _animPlayer = GetNode<AnimationPlayer>("Character/AnimationPlayer");
        if(_animPlayer == null)
            GD.PrintErr("CharacterController -> Failed to get reference to the animation player");

        // Create and start the new animator
        _anim = new Animator();
        _anim.OnStart(this);

        FindRandomPathPointInWorld();                       // Finds a path point to follow

        Callable.From(ActorSetup).CallDeferred();
    }

    public override void _Process(double dt)
    {
        base._Process(dt);
        // Update the state machine
        if(_stateMachine != null)
            _stateMachine.OnUpdate((float)dt);
        
        // Update the animator
        if(_anim != null)
            _anim.OnUpdate((float)dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        HandleMovement((float)delta);                   // Handle movement
    }

    /// <summary>
    /// Handles moving the character to the target location if it is set
    /// </summary>
    /// <param name="dt"></param>
    private void HandleMovement(float dt)
    {
        // Check that we can move
        if (!_hasMoveToLocation || _agent == null || _agent.IsNavigationFinished())
            return;

        var currentPos = GlobalTransform.Origin;                    // Get reference to the current position
        var nextPathPosition = _agent.GetNextPathPosition();            // Get reference to the next point in the paht

        Velocity = currentPos.DirectionTo(nextPathPosition) * _generalMovementSpeed;                // Apply the velocity
        MoveAndSlide();                 // Move the character

        // Handle rotating the character in the movement direction
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
        MoveToLocation = moveTo;                        // Set the move to location
        _hasMoveToLocation = moveTo != Vector3.Zero;                // Determine if we have a location to move to
        
        // update the animator if we are moving or not
        if(_anim != null)
            _anim.SetStateProperty("IsMoving", _hasMoveToLocation);
    }

    public async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        CreateStateMachine();
    }

    /// <summary>
    /// Creates and starts the state machine
    /// </summary>
    protected virtual void CreateStateMachine()
    {
        _stateMachine = new GuardStateMachine();
        _stateMachine.OnStart(this);
    }

    /// <summary>
    /// Sets the already assigned path point
    /// </summary>
    protected void SetPathPoint()
    {
        if (_followPathPoint && _followPathController != null)
        {
            _followPathController.FollowingCharacter = this;
        }
    }
    
    /// <summary>
    /// Finds a random path within the scene for the character to follow
    /// </summary>
    protected void FindRandomPathPointInWorld()
    {
        if (_followPathPoint)
        {
            if (_followPathController == null)
            {
                RandomNumberGenerator rand = new RandomNumberGenerator();
                // Get reference to all the path points in the scene
                var pathPoints = GetTree().GetNodesInGroup("PathPoints");
                
                GD.Print(pathPoints.Count);

                if (pathPoints.Count == 0)
                    return;
                
                // Check at least 50 times for a path to follow
                for (int i = 0; i < 50; i++)
                {
                    rand.Randomize();
                    var pointIndex = rand.RandiRange(0, pathPoints.Count);                  // Determine the path index
                    var pathPoint = (PathPointController)pathPoints[pointIndex];                // Get reference to the path point
                    // Make sure the path isn't null
                    if (pathPoint != null)
                    {
                        // Check if we can assign this character to the path
                        if (pathPoint.FollowingCharacter == null || pathPoint.FollowingCharacter == this)
                        {
                            pathPoint.FollowingCharacter = this;
                            return;
                        }
                    }
                }

                
            }
        }
        
        if (_followPathController == null)
        {
            GD.Print("CharacterController -> Failed to find path to follow");
            _followPathPoint = false;
            StopFollowPath();
        }
    }

    protected void StopFollowPath()
    {
        // TODO: Disable the follow path
    }
}