using System.Collections.Generic;
using Godot;

public partial class CameraController : Node3D
{
    private Camera3D _camRef;                       // Store reference to the camera

    public override void _Ready()
    {
        base._Ready();
        _camRef = GetNode<Camera3D>("Camera3D");
        if(_camRef == null)
            GD.PrintErr("CameraController -> Failed to get reference to the camera");
    }

    public override void _Process(double dt)
    {
        base._Process(dt);
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        
    }
}