using System.Collections.Generic;
using Godot;

public partial class CoverPointController : Node3D
{
    private List<Vector3> _coverPoints = new List<Vector3>();                   // Store reference to the cover points
    private CharacterController _usingCharacter = null;                         // Which character is currently using this point

    public bool IsInUse => _usingCharacter != null;
    public override void _Ready()
    {
        base._Ready();
        var points = GetChildren();
        foreach (var point in points)
            if (point is Node3D node)
                _coverPoints.Add(node.GlobalPosition);
    }

    public Vector3 GetClosesCoverPointToSelf(Vector3 currentPos)
    {
        Vector3 closesPoint;
        closesPoint = _coverPoints[0];

        foreach(var point in _coverPoints)
        {
            var distance = currentPos.DistanceTo(point);
            if (distance < currentPos.DistanceTo(closesPoint))
                closesPoint = point;
        }

        return closesPoint;
    }

    public Vector3 GetClosesCoverPointToTarget(Vector3 currentPos, Vector3 targetPos)
    {
        Vector3 closesPoint;
        closesPoint = _coverPoints[0];

        foreach(var point in _coverPoints)
        {
            var distance = targetPos.DistanceTo(point);
            if (distance < targetPos.DistanceTo(closesPoint))
                closesPoint = point;
        }

        return closesPoint;
    }

    public Vector3 GetEndPoint(Vector3 currentPos)
    {
        float distanceOne = currentPos.DistanceTo(_coverPoints[0]);
        float distanceTwo = currentPos.DistanceTo(_coverPoints[_coverPoints.Count - 1]);

        return distanceOne < distanceTwo ? _coverPoints[0] : _coverPoints[_coverPoints.Count - 1];
    }
}