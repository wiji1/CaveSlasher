using System;

public class StateManager<T> where T : Enum
{
    private readonly IStateful<T> _owner;
    private T _currentState;
    
    public StateManager(IStateful<T> owner, T initialState)
    {
        _owner = owner;
        _currentState = initialState;
        _owner.OnStateEnter(initialState);
    }
    
    public T CurrentState => _currentState;
    
    public bool TryChangeState(T newState)
    {
        if (_currentState.Equals(newState) || !_owner.CanTransitionTo(newState)) return false;
            
        _owner.OnStateExit(_currentState);
        _currentState = newState;
        _owner.OnStateEnter(newState);
        return true;
    }
}