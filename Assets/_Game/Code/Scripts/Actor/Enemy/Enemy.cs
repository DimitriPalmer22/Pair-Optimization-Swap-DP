using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IActor
{
    [SerializeField] private ActorInfo actorInfo;

    [SerializeField] public UnityEvent<Enemy> onDeath;

    public ActorInfo ActorInfo => actorInfo;

    private void Awake()
    {
        // Subscribe to the health value changed event
        actorInfo.Health.OnValueChanged.AddListener(CheckForDeath);
    }

    private void Start()
    {
        // Set the initial health value
        actorInfo.Health.ForceValue(actorInfo.Health.MaxValue);
    }

    private void Update()
    {
        // Tick the invincibility timer
        actorInfo.InvincibilityTimer.ChangeValue(-Time.deltaTime);
    }

    public void CheckForDeath(RangedValue arg0)
    {
        // Return if the health is over 0
        if (actorInfo.Health.CurrentValue > 0)
            return;

        // Invoke the death event
        onDeath.Invoke(this);

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

    #region Public Functions

    public void LogHealth()
    {
        // Log the current health value
        Debug.Log($"{gameObject.name} Health: {actorInfo.Health.CurrentValue:0.00}", this);
    }

    [Button]
    public void ChangeHealth(float amount)
    {
        // If the invincibility timer is active,
        // and the amount is negative, return
        if (actorInfo.InvincibilityTimer.CurrentValue > 0 && amount < 0)
            return;

        actorInfo.Health.ChangeValue(amount);
    }

    public void SetInvincibleForSeconds(float seconds)
    {
        // Set the invincibility timer to the specified seconds
        actorInfo.InvincibilityTimer.SetValue(seconds);
    }

    #endregion
}