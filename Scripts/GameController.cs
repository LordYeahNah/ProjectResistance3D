using Godot;
using System;

public enum ETimeOfDay
{
	TIME_Day,
	TIME_Night
}

public partial class GameController : Node
{
	public ETimeOfDay TimeOfDay;
}
