using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class EnemyAttack : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        // Get the components
        GetComponents();

        // Custom Awake
        CustomAwake();
    }

    protected abstract void CustomAwake();

    private void GetComponents()
    {
        // Get the enemy component
        _enemy = GetComponent<Enemy>();
    }
}