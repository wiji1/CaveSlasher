using System;
using UnityEngine;

public class AnimationController<T> : IAnimationController<T> where T : Enum
{
    private readonly Animator _animator;

    public AnimationController(Animator animator) => _animator = animator;

    public void SetBool(T parameter, bool value) => _animator.SetBool(parameter.ToString(), value);
    public void SetTrigger(T parameter) => _animator.SetTrigger(parameter.ToString());

    public void SetBool(DefaultAnimationParameter animationParameter, bool value) => _animator.SetBool(animationParameter.ToString(), value);
    public void SetTrigger(DefaultAnimationParameter animationParameter) => _animator.SetTrigger(animationParameter.ToString());
}