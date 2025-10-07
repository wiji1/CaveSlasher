using System;
using Interfaces;
using UnityEngine;

public abstract class Enemy : Combatant
{
    private const float DetectionRadius = 8f;
    
    private Action<bool> _updateCallback;
    
    private AnimationController<EnemyAnimationParameter> _animationController;
    private CombatantAudioController<EnemySound> _audioController;
    
    private Player _player;
    private Vector3 _baseScale;
    
    protected override void OnAwake()
    {
        _updateCallback = Pathfind;
        _animationController = new AnimationController<EnemyAnimationParameter>(GetAnimator());
        _audioController = new CombatantAudioController<EnemySound>(GetAudioSource(), GetAudioPath());
        _player = Player.GetPlayer();
        _baseScale = transform.localScale;
        UpdateConsumer(_updateCallback);
    }
    
    protected abstract void UpdateConsumer(Action<bool> action);
    
    protected abstract void Move(Vector3 targetPosition);
    
    protected abstract Animator GetAnimator();
    
    protected abstract AudioSource GetAudioSource();

    protected abstract string GetAudioPath();

    protected override IAnimationController GetAnimationController()
    {
        return _animationController;
    }
    
    protected override ICombatantAudioController GetAudioController()
    {
        return _audioController;
    }
    
    protected override void PerformAttack(IDamageable target)
    {
        _animationController.SetTrigger(EnemyAnimationParameter.Attack);
        _audioController.PlaySound(EnemySound.Attack);
        
        target.Damage(this);
    }

    private void Pathfind(bool success)
    {
        if (!success) return;
        if (CurrentCombatantState() == CombatantState.Dead) return;
        
        var playerBounds = _player.Collider().bounds;
        var enemyBounds = Collider().bounds;
        
        var distance = Vector2.Distance(playerBounds.center, enemyBounds.center);
        var enemyDiameter = enemyBounds.extents.magnitude;

        if (distance > DetectionRadius) return;
        
        if (distance > enemyDiameter + GetStats().AttackRange)
        {
            Move(_player.GetPlayerObject().transform.position);
            _animationController.SetBool(EnemyAnimationParameter.IsMoving, true);
            FlipSprite();
        }
        else
        {
            TryAttack(_player);
            _animationController.SetBool(EnemyAnimationParameter.IsMoving, false);
        }
    }
    
    private void FlipSprite()
    {
        var currentPosition = transform.position;
        var directionToPlayer = _player.GetPlayerObject().transform.position - transform.position;
        
        if (Mathf.Abs(directionToPlayer.x) > 0.01f)
        {
            if (directionToPlayer.x > 0) transform.localScale = new Vector3(Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
            else transform.localScale = new Vector3(-Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
        }
    }
}