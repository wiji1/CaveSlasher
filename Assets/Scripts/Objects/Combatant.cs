using System;
using System.Collections;
using UnityEngine;
using Interfaces;

public abstract class Combatant : MonoBehaviour, IDamageSource, IDamageable, IStateful<CombatantState>, IStateful<KnockbackState>
{
    private CombatantStats _stats;
    
    private HealthComponent _health;
    private CombatController _combat;
    private StateManager<CombatantState> _combatantStateManager;
    private StateManager<KnockbackState> _knockbackStateManager;
    
    protected HealthComponent Health() => _health;
    protected CombatController Combat() => _combat;
    
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private IAnimationController _animationController;
    private ICombatantAudioController _audioController;
    
    public CombatantState CurrentCombatantState() => _combatantStateManager.CurrentState;
    public KnockbackState CurrentKnockbackState() => _knockbackStateManager.CurrentState;
    
    public event Action<int, int> OnHealthChanged;
    public event Action<IDamageSource> OnDamaged;
    public event Action OnDied;
    public event Action OnAttackStarted;
    public event Action OnAttackFinished;
    public event Action OnKnockbackStarted;
    public event Action OnKnockbackFinished;

    protected virtual void Awake()
    {
        InitializeCombatant();
    }

    private void InitializeCombatant()
    {
        OnAwake();
        
        _stats = GetStats();
        _health = new HealthComponent(_stats.MaxHealth);
        _health.OnHealthChanged += (current, max) => OnHealthChanged?.Invoke(current, max);
        _health.OnDamaged += OnHealthDamaged;
        _health.OnDeath += OnHealthDepleted;
        
        _combat = new CombatController(this, _stats.AttackCooldown);

        _collider = GetCollider();
        _rigidbody = GetRigidbody();
        
        _animationController = GetAnimationController();
        _audioController = GetAudioController();
        
        _combatantStateManager = new StateManager<CombatantState>(this, CombatantState.Alive);
        _knockbackStateManager = new StateManager<KnockbackState>(this, KnockbackState.Inactive);
    }

    public virtual void Damage(IDamageSource damageSource)
    {
        if (!CanTakeDamage(damageSource)) return;
        
        _health.TakeDamage(damageSource);
        if (_health.IsDead()) return;
        
        if (damageSource.ShouldApplyKnockback())
        {
            var knockbackDirection = CalculateKnockback(damageSource);
            ApplyKnockback(knockbackDirection);
            
            _knockbackStateManager.TryChangeState(KnockbackState.Active);
        }
        
        if (_stats.InvulnerabilityDuration > 0f) StartCoroutine(InvulnerabilityFrames());
    }

    public Vector3 GetPosition() => transform.position;

    public bool TryAttack(IDamageable target)
    {
        return _combat.TryAttack(PerformAttack, target);
    }

    public int GetDamageAmount()
    {
        return _stats.AttackDamage;
    }

    public bool ShouldApplyKnockback()
    {
        return _stats.KnockbackForce > 0f;
    }
    
    public float GetKnockbackForce()
    {
        return _stats.KnockbackForce;
    }

    protected abstract void OnAwake();
    protected abstract CombatantStats GetStats();
    protected abstract void PerformAttack(IDamageable target);
    protected abstract void OnDeath();
    public abstract Collider2D GetCollider();
    protected abstract Rigidbody2D GetRigidbody();
    protected abstract IAnimationController GetAnimationController();
    protected abstract ICombatantAudioController GetAudioController();

    protected virtual void ApplyKnockback(Vector2 direction)
    {
        _rigidbody.linearVelocity = direction;

    }
    
    protected virtual bool CanTakeDamage(IDamageSource source)
    {
        return CurrentCombatantState() != CombatantState.Dead && 
               CurrentCombatantState() != CombatantState.Invulnerable;
    }
    
    protected virtual Vector2 CalculateKnockback(IDamageSource source)
    {
        if (source is Combatant sourceCombatant)
        {
            var attackerFacing = sourceCombatant._rigidbody.transform.localScale.x;
            var knockbackDirection = new Vector2(attackerFacing > 0 ? 1 : -1, 0.3f).normalized;
        
            return knockbackDirection * source.GetKnockbackForce();
        }
    
        var direction = ((Vector2)_rigidbody.transform.position - (Vector2)source.GetPosition()).normalized;
        if (direction.magnitude < 0.001f) direction = Vector2.right;
    
        return new Vector2(direction.x, 0.3f).normalized * source.GetKnockbackForce();
    }

    public bool CanTransitionTo(CombatantState newState)
    {
        return newState switch
        {
            CombatantState.Dead => CurrentCombatantState() == CombatantState.Alive,
            CombatantState.Invulnerable => CurrentCombatantState() == CombatantState.Alive,
            CombatantState.Alive => CurrentCombatantState() == CombatantState.Invulnerable,
            _ => false
        };
    }
    

    public void OnStateEnter(CombatantState state)
    {
        switch (state)
        {
            case CombatantState.Dead:
                OnDeath();
                _animationController.SetTrigger(DefaultAnimationParameter.Death);
                _audioController.PlaySound(CombatantSound.Death);
              
                if (this is not Player)
                {
                    StartCoroutine(DespawnCoroutine());
                    _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                    _rigidbody.linearVelocity = Vector2.zero;
                    _collider.isTrigger = true;
                }
                
                break;
            default: return;
        }
        return;
        
        IEnumerator DespawnCoroutine()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }

    public void OnStateExit(CombatantState state) { }
    
    public bool CanTransitionTo(KnockbackState newState)
    {
        return newState switch
        {
            KnockbackState.Active => CurrentKnockbackState() == KnockbackState.Inactive,
            KnockbackState.Inactive => CurrentKnockbackState() == KnockbackState.Active,
            _ => false
        };
    }

    public void OnStateEnter(KnockbackState state)
    {
        switch (state)
        {
            case KnockbackState.Active:
                StartCoroutine(KnockbackCoroutine());
                _animationController.SetTrigger(DefaultAnimationParameter.Hit);
                _audioController.PlaySound(CombatantSound.Hurt);
                
                _animationController.SetBool(DefaultAnimationParameter.IsKnockedBack, true);
                OnKnockbackStarted?.Invoke();
                break;
            case KnockbackState.Inactive:
                _animationController.SetBool(DefaultAnimationParameter.IsKnockedBack, false);
                OnKnockbackFinished?.Invoke();
                break;
            default: return;
        }

        return;

        IEnumerator KnockbackCoroutine()
        {
            yield return new WaitForSeconds(_stats.KnockbackDuration);
            _knockbackStateManager.TryChangeState(KnockbackState.Inactive);
        }
    }


    public void OnStateExit(KnockbackState state) { }

    public bool CanAttack() => CurrentCombatantState() != CombatantState.Dead && !_combat.IsOnCooldown();
    public void OnCombatStarted() => OnAttackStarted?.Invoke();
    public void OnCombatFinished() => OnAttackFinished?.Invoke();

    private void OnHealthDamaged(IDamageSource source)
    {
        OnDamaged?.Invoke(source);
    }

    private void OnHealthDepleted()
    {
        _combatantStateManager.TryChangeState(CombatantState.Dead);
        OnDied?.Invoke();
    }

    private IEnumerator InvulnerabilityFrames()
    {
        _combatantStateManager.TryChangeState(CombatantState.Invulnerable);
        yield return new WaitForSeconds(_stats.InvulnerabilityDuration);
        _combatantStateManager.TryChangeState(CombatantState.Alive);
    }

    public bool IsAlive() => _health.IsAlive();
    public bool IsDead() => _health.IsDead();
    public int CurrentHealth() => _health.CurrentHealth();
    public int MaxHealth() => _health.MaxHealth();
    
    public Collider2D Collider() => _collider;
    public Rigidbody2D Rigidbody() => _rigidbody;
}