using System;
using Interfaces;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    private Action<bool> _updateCallback;
    
    protected override void UpdateConsumer(Action<bool> action)
    {
        _updateCallback = action;
    }

    protected override CombatantStats GetStats() => new(
        maxHealth: 2, 
        attackRange: 1.0f, 
        attackCooldown: 1.5f, 
        invulnerabilityDuration: 0f, 
        attackDamage: 1, 
        knockbackForce: 8f,
        knockbackDuration: 0.5f
    );
    
    protected override void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2f * Time.deltaTime);
    }
    
    protected override void OnDeath()
    {
      
    }
    
    public override Collider2D GetCollider()
    {
        return transform.GetChild(0).GetComponent<Collider2D>();
    }

    protected override Rigidbody2D GetRigidbody()
    {
        return GetComponent<Rigidbody2D>();
    }

    protected override Animator GetAnimator()
    {
        return transform.GetChild(0).GetComponent<Animator>();
    }
    
    protected override AudioSource GetAudioSource()
    {
        return transform.GetChild(0).GetComponent<AudioSource>();
    }
    
    protected override string GetAudioPath()
    {
        return "Enemies/Slime";
    }

    private void Update()
    {
        _updateCallback?.Invoke(true);
    }
}
