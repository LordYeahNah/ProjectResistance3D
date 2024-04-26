using System.Collections.Generic;
using System.Linq;
using Godot;
using NexusExtensions;

public class DeteremineNextCombatState : State
{
    public Dictionary<float, State> PotentialStates = new Dictionary<float, State>();
    RandomNumberGenerator _rand;
    public DeteremineNextCombatState(StateMachine stateMach, bool hasExit = false, bool loop = false) : base(stateMach, hasExit, loop)
    {
        _rand = new RandomNumberGenerator();
    }

    public DeteremineNextCombatState(StateMachine stateMach, SubStateMachine subState, bool hasExit = false, bool loop = false) : base(stateMach, subState, hasExit, loop)
    {
        _rand = new RandomNumberGenerator();
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        _rand.Randomize();

        var possibleStates = new Dictionary<float, State>();

        foreach(var s in PotentialStates)
        {
            if (s.Key >= 1.0f)
            {
                SetState(s.Value);
                return;
            }

            if (s.Key > _rand.Randf())
                possibleStates.Add(s.Key, s.Value);
        }

        if(possibleStates.Count > 0)
        {
            var loopCount = 0;
            while(true)
            {
                Dictionary<float, State> tempDic = new Dictionary<float, State>(possibleStates);
                foreach(var s in tempDic)
                {
                    _rand.Randomize();
                    if(possibleStates.Count == 1)
                    {
                        SetState(possibleStates[0]);
                        return;
                    }

                    if (s.Key < _rand.Randf())
                        possibleStates.Remove(s.Key);
                }

                loopCount += 1;
                if (loopCount > 50)
                    break;
            }

            if(possibleStates.Count > 0)
            {
                _rand.Randomize();
                SetState(possibleStates[_rand.RandiRange(0, possibleStates.Count - 1)]);
                return;
            }

        }

        SetState(PotentialStates[_rand.RandiRange(0, PotentialStates.Count - 1)]);

    }

    private void SetState(State state)
    {
        if(IS_OWNED_BY_SUBSTATE)
        {
            _subState.SetState(state);
        } else
        {
            _stateMachine.SetState(state);
        }
    }
}