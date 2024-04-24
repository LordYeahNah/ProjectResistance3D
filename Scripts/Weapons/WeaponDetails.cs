using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using Godot;
using Newtonsoft.Json;

public enum EWeaponType
{
    WPN_Melee = 0,
    WPN_Rifle = 1,
    WPN_Shotgun = 2,
    WPN_Pistol = 3,
}

public class WeaponDetails
{
    // === Weapon Details === //
    // Store reference to the weapon ID
    public readonly string WEAPON_ID;
    public string WeaponName;                           // Name of the weapon     
    public EWeaponType WeaponType;                          // Type of the weapon

    // === Weapon Settings === //
    protected float _damagePoints;
    protected float _criticalHitChance;
    protected float _criticalHitModifier;
    protected float _weaponCooldown;                    // How long before the weapon can be used again
    public float WeaponCooldown => _weaponCooldown;

    // === Ammo Settings === //
    protected int _maxAmmoInMag;
    public int MaxAmmoInMag => _maxAmmoInMag;

    public WeaponDetails(JsonWeapon weapon)
    {
        WEAPON_ID = weapon.WeaponID;
        WeaponName = weapon.WeaponName;
        WeaponType = (EWeaponType)weapon.WeaponType;

        _damagePoints = weapon.DamagePoints;
        _criticalHitChance = weapon.CriticalHitChance;
        _criticalHitModifier = weapon.CriticalHitModifier;
        _weaponCooldown = weapon.WeaponCooldown;

        _maxAmmoInMag = weapon.AmmoInMag;
    }

    /// <summary>
    /// Calculates the damage points
    /// </summary>
    /// <param name="modifier">Only modifier to apply</param>
    /// <returns>Damage points for a hit</returns>
    public float CalculateDamagePoints(float modifier)
    {
        float dp = _damagePoints * modifier;            // Create the initial damage points
        // Create the number generator
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        // Determine if a critical hit
        bool isCritHit = rand.Randi() < _criticalHitChance;

        // Apply damage modifier if it is a critical hit
        if (isCritHit)
            dp *= _criticalHitModifier;
        return dp;
    }

    /// <summary>
    /// Calculate damage points with multiple modifiers
    /// </summary>
    /// <param name="modifiers">List of modifiers to apply</param>
    /// <returns>Damage points for a hit</returns>
    public float CalculateDamagePoints(List<float> modifiers)
    {
        float dp = _damagePoints;
        foreach (var m in modifiers)
            dp *= m;

        // Create the number generator
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        // Determine if a critical hit
        bool isCritHit = rand.Randi() < _criticalHitChance;

        // Apply damage modifier if it is a critical hit
        if (isCritHit)
            dp *= _criticalHitModifier;

        return dp;
    }
}

public class JsonWeapon
{
    [JsonProperty]
    public string WeaponID;
    [JsonProperty]
    public string WeaponName;
    [JsonProperty]
    public int WeaponType;
    [JsonProperty]
    public float DamagePoints;
    [JsonProperty]
    public float CriticalHitChance;
    [JsonProperty]
    public float CriticalHitModifier;
    [JsonProperty]
    public float WeaponCooldown;
    [JsonProperty]
    public int AmmoInMag;
}