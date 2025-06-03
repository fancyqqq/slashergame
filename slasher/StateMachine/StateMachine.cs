using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public class StateMachine
{
    private Dictionary<Type, IState> _states;
    public IState currentStates { get; private set; }
    public bool isUpdate { get; private set; }

    public StateMachine(params IState[] states)
    {
        _states = new Dictionary<Type, IState>(states.Length);
        foreach (var state in states)
            _states.Add(state.GetType(), state);
    }

    public void SwitchStates<TState>() where TState : IState
    {
        if (currentStates is TState newState)
            return;
        isUpdate = false;
        TryExitStates();
        GetNewState<TState>();
        TryEnterStates<TState>();
        isUpdate = true;
    }

    private void TryEnterStates<TState>() where TState : IState
    {
        if (currentStates is TState newState)
            newState.OnEnter();
    }

    private void TryExitStates()
    {
        currentStates?.OnExit();
    }

    private void GetNewState<TState>() where TState : IState
    {
        currentStates = GetState<TState>();
    }

    private TState GetState<TState>() where TState : IState
    {
        return (TState)_states[typeof(TState)];
    }
}
