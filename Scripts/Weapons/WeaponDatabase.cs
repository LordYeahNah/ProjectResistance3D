using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class WeaponDatabase : Node
{
    public static readonly string FILE_PATH = "res://Database/Weapons.json";
    private List<WeaponDetails> _weapons = new List<WeaponDetails>();

    public override void _Ready()
    {
        base._Ready();
        LoadWeapons();

        foreach (var weapon in _weapons)
            GD.Print(weapon.WeaponName);
    }

    private void LoadWeapons()
    {
        var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read);
        if(file != null && file.IsOpen())
        {
            var data = file.GetAsText();
            var fileJson = JArray.Parse(data);
            foreach(var jData in fileJson)
            {
                var token = jData;
                var weaponJson = JsonConvert.DeserializeObject<JsonWeapon>(token.ToString());
                var weaponDetails = new WeaponDetails(weaponJson);
                if(weaponDetails != null)
                {
                    _weapons.Add(weaponDetails);
                }
            }
        }
    }

    public WeaponDetails GetWeapon(string id, bool usingName = true)
    {
        if(usingName)
        {
            foreach (var wpn in _weapons)
                if (wpn.WeaponName == id)
                    return wpn;
        } else
        {
            foreach (var wpn in _weapons)
                if (wpn.WEAPON_ID == id)
                    return wpn;
        }

        return null;
    }
}