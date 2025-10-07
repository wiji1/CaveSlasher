using UnityEngine;

public interface IDamageSource
{
    int GetDamageAmount();
        
    bool ShouldApplyKnockback();
        
    float GetKnockbackForce();
        
    Vector3 GetPosition();
}