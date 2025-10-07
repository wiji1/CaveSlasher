using System;

public interface IStateful<T> where T : Enum
{
    bool CanTransitionTo(T newState);
    void OnStateEnter(T state);
    void OnStateExit(T state);
}