using System;
using Interfaces;
using UnityEngine;

public class HealthComponent
{
    private int _currentHealth;
    private readonly int _maxHealth;
    
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    public event Action<IDamageSource> OnDamaged;
    
    public HealthComponent(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }
    
    public int CurrentHealth() => _currentHealth;
    public int MaxHealth() => _maxHealth;
    public bool IsAlive() => _currentHealth > 0;
    public bool IsDead() => _currentHealth <= 0;
    
    public void TakeDamage(IDamageSource source)
    {
        if (IsDead()) return;
        
        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Max(0, _currentHealth - source.GetDamageAmount());
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        OnDamaged?.Invoke(source);
        
        if (_currentHealth <= 0 && oldHealth > 0)
            OnDeath?.Invoke();
    }
    
    public void Heal(int amount)
    {
        if (IsDead()) return;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}