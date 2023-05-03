using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine
{
    public BaseState currentState;
    private Dictionary<CharacterState, BaseState> states = new Dictionary<CharacterState, BaseState>();


    public void AddStates()
    {
        states.Add(CharacterState.Idle, new Idle());
        states.Add(CharacterState.Walking, new Walking());
        states.Add(CharacterState.Running, new Running());
        states.Add(CharacterState.SlowRunning, new SlowRunning());
        states.Add(CharacterState.Jumping, new Jumping());
    }

    public void SetState(CharacterState stateType)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = states[stateType];
        currentState.EnterState();
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.UpdateState();
    }
}

