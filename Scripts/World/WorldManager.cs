using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json.Serialization;

public enum EFactions
{
    FACT_Resitance,
    FACT_Civilian,
    FACT_Soldier
}

public partial class WorldManager : Node
{
    private List<World> _worlds = new List<World>();
    public World ActiveWorld;

    public override void _Process(double delta)
    {
        base._Process(delta);

        // Update each world
        foreach(var world in _worlds)
        {
            if (world != ActiveWorld)
                world.OnUpdate((float)delta);
        }
    }

    public List<CharacterController> GetReistanceMembers(World world)
    {
        if(world == ActiveWorld)
        {
            List<CharacterController> charactersInGroup = new List<CharacterController>();
            var characters = GetTree().GetNodesInGroup("ResistanceCharacter");
            foreach(var character in characters)
            {
                if (character is CharacterController cc)
                    charactersInGroup.Add(cc);
            }


            return charactersInGroup;
        }

        return world.ResistanceGroup;
    }

    public List<CharacterController> GetSoldierGroup(World world)
    {
        if (world == ActiveWorld)
        {
            List<CharacterController> charactersInGroup = new List<CharacterController>();
            var characters = GetTree().GetNodesInGroup("SoldierGroup");
            foreach (var character in characters)
            {
                if (character is CharacterController cc)
                    charactersInGroup.Add(cc);
            }


            return charactersInGroup;
        }

        return world.SoldierEnemies;
    }

    public List<CharacterController> GetCivilanGroup(World world)
    {
        if (world == ActiveWorld)
        {
            List<CharacterController> charactersInGroup = new List<CharacterController>();
            var characters = GetTree().GetNodesInGroup("Civilian");
            foreach (var character in characters)
            {
                if (character is CharacterController cc)
                    charactersInGroup.Add(cc);
            }


            return charactersInGroup;
        }

        return world.CiviliansGroup;
    }

    
}