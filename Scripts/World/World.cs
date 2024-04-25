using System.Collections.Generic;
using Godot;

public class World
{
    public string WorldName;
    public List<CharacterController> CiviliansGroup = new List<CharacterController>();
    public List<CharacterController> ResistanceGroup = new List<CharacterController>();
    public List<CharacterController> SoldierEnemies = new List<CharacterController>();

    public World(string name)
    {
        WorldName = name;
    }

    public void OnEnter()
    {

    }

    public virtual void OnUpdate(float dt)
    {

    }

    /// <summary>
    /// Adds a character to the specified faction
    /// </summary>
    /// <param name="faction">Faction to add to</param>
    /// <param name="world">World the character exist in</param>
    /// <param name="cc">Character to add</param>
    public void AddToFaction(EFactions faction, CharacterController cc)
    {
        switch (faction)
        {
            case EFactions.FACT_Resitance:
                ResistanceGroup.Add(cc);
                foreach (var character in SoldierEnemies)
                    character.Sight.Enemies.Add(cc);
                return;
            case EFactions.FACT_Civilian:
                CiviliansGroup.Add(cc);
                return;
            case EFactions.FACT_Soldier:
                SoldierEnemies.Add(cc);
                foreach (var character in ResistanceGroup)
                    character.Sight.Enemies.Add(cc);
                return;
            default:
                GD.PrintErr("WorldManager -> Failed to add to a faction");
                return;

        }
    }

    /// <summary>
    /// Removes an enemy from the faction
    /// </summary>
    /// <param name="faction">Faction to remove from</param>
    /// <param name="cc">Character to remove</param>
    public void RemoveFromFaction(EFactions faction, CharacterController cc)
    {
        switch(faction)
        {
            case EFactions.FACT_Resitance:
                if(ResistanceGroup.Contains(cc))
                    ResistanceGroup.Remove(cc);
                foreach (var character in SoldierEnemies)
                    if (character.Sight.Enemies.Contains(cc))
                        character.Sight.Enemies.Remove(cc);
                break;
            case EFactions.FACT_Civilian:
                if (CiviliansGroup.Contains(cc))
                    CiviliansGroup.Remove(cc);
                break;
            case EFactions.FACT_Soldier:
                if (SoldierEnemies.Contains(cc))
                    SoldierEnemies.Remove(cc);
                foreach (var character in ResistanceGroup)
                    if (character.Sight.Enemies.Contains(cc))
                        character.Sight.Enemies.Remove(cc);
                break;
        }
    }
}