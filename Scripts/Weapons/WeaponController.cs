using Godot;
using NexusExtensions;
using Timer = NexusExtensions.Timer;

public enum EWeaponState
{
    WPNSTATE_Idle,
    WPNSTATE_Reload,
    WPNSTATE_FIRING,
    WPNSTATE_COOLDOWN
}

public partial class WeaponController : Node3D
{
    // === Weapon Details === //
    protected WeaponDetails _weapon;
    public WeaponDetails Weapon => _weapon;
    protected EWeaponState _weaponState;

    // === Ammo Details === //
    protected int _currentAmmoInMag;

    // === Components === //
    protected Timer _cooldownTimer;

    /// <summary>
    /// Sets up the new weapon
    /// </summary>
    /// <param name="weapon">Weapon to attach</param>
    public void Setup(WeaponDetails weapon)
    {
        _weapon = weapon;
        _currentAmmoInMag = _weapon.MaxAmmoInMag;
        _cooldownTimer = new Timer(_weapon.WeaponCooldown, false, ResetWeaponState, false);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_cooldownTimer != null)
            _cooldownTimer.OnUpdate((float)delta);
    }

    public void Fire()
    {
        if(CanFire())
        {
            GD.Print("Fired Weapon");
            if (_cooldownTimer != null)
                _cooldownTimer.IsActive = true;

            _weaponState = EWeaponState.WPNSTATE_COOLDOWN;
            // TODO: Fire Weapon
        }
    }

    public void ResetWeaponState()
    {
        _weaponState = EWeaponState.WPNSTATE_Idle;
    }

    protected bool CanFire()
    {
        if(_weaponState == EWeaponState.WPNSTATE_Idle)
        {
            if(_currentAmmoInMag > 0)
            {
                return true;
            } else
            {
                Reload();
            }
        }

        return false;
    }

    protected void Reload()
    {
        _weaponState = EWeaponState.WPNSTATE_Reload;
        // TODO: Perform reload animation
        
    }
}
