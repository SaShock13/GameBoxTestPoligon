using System;
using System.Collections.Generic;

public class StateMachine 
{
    public State CurrentState { get; private set; }

    private Dictionary<Type,State> _states = new Dictionary<Type, State>();


    public void AddState(State state)
    {
        _states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : State
    {
        Type type = typeof(T);

        if (CurrentState != null && CurrentState.GetType() == type)
        {
            return;
        }
        if (_states.TryGetValue(type, out var newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }
    }
    public void Update()
    {
        CurrentState?.Update();
    }
}
