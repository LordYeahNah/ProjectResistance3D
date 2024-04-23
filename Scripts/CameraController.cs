using System.Collections.Generic;
using Godot;

public partial class CameraController : Node3D
{
    private Camera3D _camRef;                       // Store reference to the camera

    [Export]
    private float _movementSpeed;                   // Speed the camera will move at

    [Export]
    private float _rotationSpeed;                   // Speed the character will rotate at

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
        HandleCameraMovement((float)dt);
        RotateCamera((float)dt);
    }

    private void HandleCameraMovement(float dt)
    {
        var inputValue = Input.GetVector("MoveRight", "MoveLeft", "MoveForward", "MoveBack");
        if (inputValue.Length() > 0.1f)
        {
            var forward = Transform.Basis.X * (inputValue.Y * (_movementSpeed * dt));
            var right = Transform.Basis.Z * (inputValue.X * (_movementSpeed * dt));

            this.Position += (forward + right);
        }
    }

    private void RotateCamera(float dt)
    {
        var input = Input.GetActionStrength("RotateRight") - Input.GetActionStrength("RotateLeft");
        var rotRate = input * _rotationSpeed * dt;
        
        RotateY(rotRate);
    }
}