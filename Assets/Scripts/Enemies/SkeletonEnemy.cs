using System;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    private Action<bool> _updateCallback;
    
    protected override void UpdateConsumer(Action<bool> action)
    {
        _updateCallback = action;
    }

    protected override CombatantStats GetStats() => new(
        maxHealth: 3, 
        attackRange: 0.2f, 
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
        return "Enemies/Skeleton";
    }

    private void Update()
    {
        _updateCallback?.Invoke(true);
    }
}