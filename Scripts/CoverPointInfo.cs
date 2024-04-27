using System.Collections.Generic;
using Godot;

public enum EEmergeType
{
    EMERGE_Left,
    EMERGE_Right,
    EMERGE_Top,
}

public partial class CoverPointInfo : Node3D
{
    [Export] public EEmergeType EmergeDirection;
}