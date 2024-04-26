using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class FireWeapon : State
{
    private WeaponController _weaponCtrl;
    public FireWeapon(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
    }

    public FireWeapon(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
    }

    public override void OnEnter()
    {
        // Get reference to the current weapon
        if (_stateMachine.Ctrl != null)
            _weaponCtrl = _stateMachine.Ctrl.AssignedWeapon;

        base.OnEnter();
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if(_weaponCtrl != null && _weaponCtrl.CanFire())
        {
            Node3D target = _stateMachine.GetStateProperty<Node3D>("Target");
            if (target != null && target is CharacterController character)
                _weaponCtrl.Fire(character);
            else
                GD.PrintErr("FireWeaponState -> Failed to get reference to target");

            OnFinish();
        } else
        {
            GD.Print("FireWeaponState -> Weapon controller reference isn't set");
        }
    }
}