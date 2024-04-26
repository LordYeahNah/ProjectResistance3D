using System.Collections.Generic;
using Godot;

public class CharacterStats
{
    public static readonly float MAX_HEALTH = 100f;
    private float _currentHealth;
    public bool IsAlive => _currentHealth > 0;

    public float DamageResistanceModifier = 1.0f;

    public CharacterStats()
    {
        _currentHealth = MAX_HEALTH;
    }

    public CharacterStats(float health)
    {
        _currentHealth = health;
    }

    public void TakeDamage(CharacterController target, float dp)
    {
        float newDP = dp * DamageResistanceModifier;                    // Calculate the damage points
        _currentHealth -= newDP;                // Reduce the health

        if(!IsAlive)
        {
            // TODO: Trigger death animation 
            // TODO: Update attacking characters target
            // TODO: Disable character
        } else
        {
            // TODO: Enter into combat state if not already
            // TODO: Set the target as the attacking
        }
    }
}