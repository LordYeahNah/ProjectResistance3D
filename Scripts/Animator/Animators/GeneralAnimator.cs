using System.Collections.Generic;
using Godot;
using NexusExtensions;
using NexusExtensions.Animation;
using Animation = NexusExtensions.Animation.Animation;

public class GeneralAnimator : Animator
{
    public GeneralAnimator()
    {
        CreateAnimator();
    }

    private void CreateAnimator()
    {
        SetStateProperty(GeneralAnimKeys.IS_MOVING, false, EPropertyType.PROP_Bool);
        SetStateProperty(GeneralAnimKeys.ARMED_STATE, (int)EArmedState.ARMED_Unarmed, EPropertyType.PROP_Int);
        SetStateProperty(GeneralAnimKeys.IS_IN_COVER, false, EPropertyType.PROP_Bool);
        SetStateProperty(GeneralAnimKeys.WILL_SHOOT, false, EPropertyType.PROP_Bool);
        SetStateProperty(GeneralAnimKeys.SHOOT, false, EPropertyType.PROP_Bool);
        SetStateProperty(GeneralAnimKeys.EMERGE_DIRECTION, 0, EPropertyType.PROP_Int);

        var unarmed = new UnarmedSubAnim(this, false, true);
        var rifle = new RifleSubState(this, false, true);

        var unarmedToRifle = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<int>(GeneralAnimKeys.ARMED_STATE, (int)EArmedState.ARMED_Rifle, EPropertyType.PROP_Int),
            },
            NextState = rifle
        };
        unarmed.Transitions.Add(unarmedToRifle);

        var rifleToUnarmed = new StateTransition
        {
            _requiredProps = new List<StateProperty>
            {
                new StateValue<int>(GeneralAnimKeys.ARMED_STATE, (int)EArmedState.ARMED_Unarmed, EPropertyType.PROP_Int)
            },
            NextState = unarmed
        };
        rifle.Transitions.Add(rifleToUnarmed);

        EntryState = rifle;
    }
}