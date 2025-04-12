using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IActor
{
    [SerializeField] private ActorInfo actorInfo;

    [SerializeField] private UnityEvent<Player> onDeath;

    public ActorInfo ActorInfo => actorInfo;

    private void Awake()
    {
        // Set the initial health value
        actorInfo.Health.SetValue(actorInfo.Health.MaxValue);

        // Subscribe to the health value changed event
        actorInfo.Health.OnValueChanged.AddListener(CheckForDeath);
    }

    public void CheckForDeath(RangedValue arg0)
    {
        // Return if the health is over 0
        if (actorInfo.Health.CurrentValue > 0)
            return;
        
        // Die
        Die();
    }

    private void Die()
    {
        // Destroy the enemy game object
        Destroy(gameObject);
    }

    [Button]
    private void ForceKill()
    {
        // Set the health to 0
        actorInfo.Health.SetValue(0);
    }

    public void LogHealth()
    {
        // Log the current health value
        Debug.Log($"{gameObject.name} Health: {actorInfo.Health.CurrentValue:0.00}", this);
    }

    [Button]
    public void ChangeHealth(float amount) => actorInfo.Health.ChangeValue(amount);
}