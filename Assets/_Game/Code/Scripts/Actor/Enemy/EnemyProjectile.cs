using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float damage = 25;

    [SerializeField] private RangedValue lifeTime;

    #endregion

    private Vector3 _direction;

    private void Awake()
    {
        // Set the initial lifetime value
        lifeTime.SetValue(lifeTime.MaxValue);

        // Destroy the projectile after its lifetime has expired
        lifeTime.OnValueChanged.AddListener(CheckForLifeTime);
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

    private void Update()
    {
        // Update the projectile's position based on the direction
        transform.position += _direction * (Time.deltaTime * moveSpeed);

        // Tick down the lifetime of the projectile
        lifeTime.ChangeValue(-Time.deltaTime);
    }

    public void Shoot(EnemyAttack enemyAttack, Vector3 startPosition, Vector3 direction)
    {
        // Set the current lifetime to the maximum value
        lifeTime.SetValue(lifeTime.MaxValue);

        // Move to the start position
        transform.position = startPosition;

        // Set the direction of the projectile
        _direction = direction;

        // // Modify the damage
        // damage += playerAttack.DamageAdded.CurrentValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Try to get the Player component
            // Deal damage to the player
            if (other.TryGetComponent(out Player player))
                player.ChangeHealth(-damage);
        }

        KillProjectile();
    }
}