using System.Collections.Generic;
using Godot;

public partial class SceneController : Node3D
{
    [Export] public string WorldName;
    protected World _worldOwner;                    // Reference to this world
    public World WorldOwner => _worldOwner;

    public override void _Ready()
    {
        base._Ready();

        // Get reference to the world
        _worldOwner = GetNode<WorldManager>("/root/WorldManager").GetWorld(WorldName);
        if (_worldOwner == null)
            GD.Print("SceneController -> Failed to get reference to the world owner");
        else
            _worldOwner.OnEnter();
    }
}