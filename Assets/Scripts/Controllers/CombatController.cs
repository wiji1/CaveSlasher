using System;
using System.Collections;
using Interfaces;
using UnityEngine;

public class CombatController
{
    private readonly Combatant _combatant;
    private readonly float _cooldownDuration;
    private bool _onCooldown;
    
    public CombatController(Combatant combatant, float cooldownDuration)
    {
        _combatant = combatant;
        _cooldownDuration = cooldownDuration;
    }
    
    public bool TryAttack(Action<IDamageable> attackAction, IDamageable target)
    {
        if (_onCooldown || !_combatant.CanAttack()) return false;
        
        _combatant.OnCombatStarted();
        attackAction?.Invoke(target);
        _combatant.StartCoroutine(AttackCooldown());
        return true;
    }
    
    private IEnumerator AttackCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(_cooldownDuration);
        _onCooldown = false;
        _combatant.OnCombatFinished();
    }
    
    public bool IsOnCooldown() => _onCooldown;
}