using System;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float damage = 25;

    [SerializeField] private RangedValue lifeTime;

    #endregion

    private Vector3 _direction;

    private void Awake()
    {
        // Destroy the projectile after its lifetime has expired
        lifeTime.OnValueChanged.AddListener(CheckForLifeTime);
    }

    private void Update()
    {
        // Update the projectile's position based on the direction
        transform.position += _direction * (Time.deltaTime * moveSpeed);

        // Tick down the lifetime of the projectile
        lifeTime.ChangeValue(-Time.deltaTime);
    }

    private void CheckForLifeTime(RangedValue value)
    {
        if (lifeTime.CurrentValue > 0)
            return;

        // Kill the projectile if its lifetime has expired
        KillProjectile();
    }

    private void KillProjectile()
    {
        // Destroy the projectile
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the other object has the enemy tag
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Try to get the Enemy component
            // Deal damage to the enemy
            if (other.TryGetComponent(out Enemy enemy))
                enemy.ChangeHealth(-damage);
        }
        
        // Kill the projectile
        KillProjectile();
    }

    public void Shoot(PlayerAttack playerAttack, Vector3 startPosition, Vector3 direction)
    {
        // Set the current lifetime to the maximum value
        lifeTime.SetValue(lifeTime.MaxValue);

        // Move to the start position
        transform.position = startPosition;

        // Set the direction of the projectile
        _direction = direction;
    }
}