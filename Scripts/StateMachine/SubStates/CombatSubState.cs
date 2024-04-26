using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;
using NexusExtensions;

public class CombatSubState : SubStateMachine
{
    public CombatSubState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        CreateStates();
    }

    private void CreateStates()
    {
        var findCoverState = new FindCover(_stateMachine, this, false, false);
        var idleInCover = new EmptyState(_stateMachine, this, false, true);
        var moveTo = new MoveToLocationState(_stateMachine, this, false, false);
        var shoot = new FireWeapon(_stateMachine, this, false, false);
        var determineNextState = new DeteremineNextCombatState(_stateMachine, this, false, false);

        determineNextState.PotentialStates.Add(0.3f, findCoverState);
        determineNextState.PotentialStates.Add(0.7f, idleInCover);

        var findCoverToMove = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("HasCoverPosition", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("IsAtCoverPosition", false, EPropertyType.PROP_Bool)
            },
            NextState = moveTo
        };
        

        
        var wantsToShootUpdater = new WantsToShootUpdater(idleInCover, _stateMachine);
        _stateMachine.AllStateUpdaters.Add(wantsToShootUpdater);

        var findCoverToIdle = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("HasCoverPosition", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("IsAtCoverPosition", true, EPropertyType.PROP_Bool),
            },
            NextState = idleInCover
        };

        var idleToShoot = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<bool>("WillShoot", true, EPropertyType.PROP_Bool),
                new StateValue<bool>("ShootNow", true, EPropertyType.PROP_Bool),
            },
            NextState = shoot
        };
        idleInCover.Transitions.Add(idleToShoot);
      

        findCoverState.Transitions.Add(findCoverToMove);
        findCoverState.Transitions.Add(findCoverToIdle);
    }
}