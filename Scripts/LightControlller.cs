using System.Collections.Generic;
using Godot;

public partial class LightController : Node3D
{
    private SpotLight3D _light;

    public override void _Ready()
    {
        base._Ready();
        _light = GetNode<SpotLight3D>("SpotLight3D");
        if(_light != null)
            GD.PrintErr("LightController -> Failed to get reference to spot light");
    }
    public void UpdateLights(ETimeOfDay timeOfDay)
    {
        switch (timeOfDay)
        {
            case ETimeOfDay.TIME_Day:
                if (_light != null)
                    _light.Visible = false;
                break;
            case ETimeOfDay.TIME_Night:
                if (_light != null)
                    _light.Visible = true;
                break;
            default:
                if (_light != null)
                    _light.Visible = false;
                break;
        }
    }
}