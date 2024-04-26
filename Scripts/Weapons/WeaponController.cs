using Godot;
using NexusExtensions;
using System.Security.Cryptography.X509Certificates;
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
    private static readonly float CHANCE_OF_HIT_TARGET = 0.5f;                  // Percentage of the time a target will be hit

    // === Weapon Details === //
    protected WeaponDetails _weapon;
    public WeaponDetails Weapon => _weapon;
    protected EWeaponState _weaponState;
    public CharacterController CharacterOwner;

    // === Ammo Details === //
    protected int _currentAmmoInMag;

    // === Components === //
    protected Timer _cooldownTimer;
    protected GpuParticles3D _muzzleFlash;

    public override void _Ready()
    {
        base._Ready();

        // Get reference to the muzzle flash
        _muzzleFlash = GetNode<GpuParticles3D>("MuzzleFlash");
        if (_muzzleFlash == null)
            GD.PrintErr("WeaponController -> Failed to get reference to the muzzle flash");
    }

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

    public void Fire(CharacterController target)
    {
        if (target == null)
            return;

        if(CanFire())
        {
            GD.Print("Fired Weapon");
            if (_cooldownTimer != null)
                _cooldownTimer.IsActive = true;

            _weaponState = EWeaponState.WPNSTATE_COOLDOWN;
            _currentAmmoInMag -= 1;

            // Trigger the muzzle flash
            if (_muzzleFlash != null)
                _muzzleFlash.Emitting = true;

            if (HasHitTarget(target))
            {
                if(target.Stats != null)
                    target.Stats.TakeDamage(CharacterOwner, _weapon.CalculateDamagePoints(1.0f));
            }
        }
    }

    public bool HasHitTarget(CharacterController target)
    {
        // TODO: Apply dodge modifier
        // TODO: Apply accuracy modifier

        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        return rand.Randf() < CHANCE_OF_HIT_TARGET;
    }

    public void ResetWeaponState()
    {
        _weaponState = EWeaponState.WPNSTATE_Idle;
        if (CharacterOwner != null)
            CharacterOwner.ResetAttack();
    }

    public bool CanFire()
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
