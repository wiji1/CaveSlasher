public readonly struct CombatantStats
{
    public readonly int MaxHealth;
    public readonly float AttackRange;
    public readonly float AttackCooldown;
    public readonly float InvulnerabilityDuration;
    public readonly int AttackDamage;
    public readonly float KnockbackForce;
    public readonly float KnockbackDuration;

    public CombatantStats(
        int maxHealth,
        float attackRange,
        float attackCooldown,
        float invulnerabilityDuration = 0f,
        int attackDamage = 1,
        float knockbackForce = 8f,
        float knockbackDuration = 0.2f
        )
    {
        MaxHealth = maxHealth;
        AttackRange = attackRange;
        AttackCooldown = attackCooldown;
        InvulnerabilityDuration = invulnerabilityDuration;
        AttackDamage = attackDamage;
        KnockbackForce = knockbackForce;
        KnockbackDuration = knockbackDuration;
    }
}