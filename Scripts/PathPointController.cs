using System.Collections.Generic;
using Godot;
using NexusExtensions;

public partial class PathPointController : Node3D
{
    private List<Node3D> _pathPoints = new List<Node3D>();
    [Export] private bool _circlePath;
    public bool CirclePath => _circlePath;
    private CharacterController _followingCharacter = null;                      // Reference to the character currently following this path

    public CharacterController FollowingCharacter
    {
        get => _followingCharacter;
        set
        {
            value.FollowPathController = this;
            _followingCharacter = value;
        }
    }

    public int PathCount => _pathPoints.Count;
    public override void _Ready()
    {
        base._Ready();
        
        // Get reference to the all the path points
        var children = GetChildren();
        foreach(var child in children)
            _pathPoints.Add((Node3D)child);
    }

    public Node3D GetPathPoint(int index) => _pathPoints[index];
}