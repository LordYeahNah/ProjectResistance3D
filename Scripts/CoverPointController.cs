using System.Collections.Generic;
using Godot;

public enum ECoverType
{
    COVER_None = 0,
    COVER_Side = 1,
    COVER_Top = 2
}

public partial class CoverPointController : Node3D
{
    private List<CoverPointInfo> _coverPoints = new List<CoverPointInfo>();                   // Store reference to the cover points
    private CharacterController _usingCharacter = null;                         // Which character is currently using this point
    [Export] public ECoverType CoverType;

    public bool IsInUse => _usingCharacter != null;
    public override void _Ready()
    {
        base._Ready();
        var points = GetChildren();
        foreach (var point in points)
            if (point is CoverPointInfo node)
                _coverPoints.Add(node);
    }

    public CoverPointInfo GetClosesCoverPointToSelf(Vector3 currentPos)
    {
        CoverPointInfo closesPoint;
        closesPoint = _coverPoints[0];

        foreach(var point in _coverPoints)
        {
            var distance = currentPos.DistanceTo(point.GlobalPosition);
            if (distance < currentPos.DistanceTo(closesPoint.GlobalPosition))
                closesPoint = point;
        }

        return closesPoint;
    }

    public CoverPointInfo GetClosesCoverPointToTarget(Vector3 currentPos, Vector3 targetPos)
    {
        CoverPointInfo closesPoint;
        closesPoint = _coverPoints[0];

        foreach(var point in _coverPoints)
        {
            var distance = targetPos.DistanceTo(point.GlobalPosition);
            if (distance < targetPos.DistanceTo(closesPoint.GlobalPosition))
                closesPoint = point;
        }

        return closesPoint;
    }

    public CoverPointInfo GetEndPoint(Vector3 currentPos)
    {
        float distanceOne = currentPos.DistanceTo(_coverPoints[0].GlobalPosition);
        float distanceTwo = currentPos.DistanceTo(_coverPoints[_coverPoints.Count - 1].GlobalPosition);

        return distanceOne < distanceTwo ? _coverPoints[0] : _coverPoints[_coverPoints.Count - 1];
    }
}