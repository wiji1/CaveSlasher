using System;

public interface IAnimationController
{
    void SetTrigger(DefaultAnimationParameter animationParameter);
    void SetBool(DefaultAnimationParameter animationParameter, bool value);
}

public interface IAnimationController<T> : IAnimationController where T : Enum
{
    void SetTrigger(T parameter);
    void SetBool(T parameter, bool value);
}