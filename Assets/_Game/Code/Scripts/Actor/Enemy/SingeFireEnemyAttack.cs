using System;
using Alchemy.Inspector;
using UnityEngine;

public class SingeFireEnemyAttack : EnemyAttack
{
    #region Serialized Fields

    [SerializeField, Required] private EnemyProjectile enemyProjectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private RangedValue attackCooldown;

    #endregion

    protected override void CustomAwake()
    {
        // Force the cooldown to be at the max value
        attackCooldown.SetValue(attackCooldown.MaxValue);
        
        // Subscribe to the cooldown value changed event
        attackCooldown.OnValueChanged.AddListener(FireWhenReady);
    }

    private void FireWhenReady(RangedValue arg0)
    {
        // Return if the cooldown is not ready
        if (attackCooldown.CurrentValue > 0)
            return;
        
        Shoot();
    }

    [Button]
    private void Shoot()
    {
        // Instantiate the projectile
        var projectile = Instantiate(enemyProjectilePrefab, firePoint.position, Quaternion.identity);
        projectile.Shoot(this, firePoint.position, firePoint.forward);

        // Reset the cooldown
        attackCooldown.SetValue(attackCooldown.MaxValue);
    }

    private void Update()
    {
        // Update the cooldown if the player instance is active
        if (Player.Instance == null)
            return;
        
        attackCooldown.ChangeValue(-Time.deltaTime);
    }
}