using System.Collections.Generic;
using Godot;
using NexusExtensions;

public class WantsToShootUpdater : StateUpdater
{
    private CharacterController _ctrlRef;
    public static readonly float CHANCE_TO_SHOOT_TIME = 0.4f;
    RandomNumberGenerator _rand;

    public WantsToShootUpdater(State stateRef, StateMachine stateMach) : base(stateRef, stateMach)
    {
        _rand = new RandomNumberGenerator();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(_ctrlRef == null)
        {
            _ctrlRef = _stateMachine.Ctrl;
            if (_ctrlRef == null)
                GD.PrintErr("WantsToShootUpdater -> Failed to get reference to the character controller");
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        CharacterController target = (CharacterController)_stateMachine.GetStateProperty<Node3D>(StateMachineKeys.TARGET);

        if(target != null &&_ctrlRef != null && _ctrlRef.CanFire())
        {
            _rand.Randomize();
            bool willShoot = _rand.Randf() < CHANCE_TO_SHOOT_TIME;
            if (willShoot)
            {
               
                _stateMachine.SetStateProperty<bool>("WillShoot", true);
                _ctrlRef.TriggerWillShoot(target);
            }
        }
    }
}