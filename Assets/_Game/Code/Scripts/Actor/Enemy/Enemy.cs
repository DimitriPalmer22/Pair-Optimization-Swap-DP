using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IActor
{
    #region Serialized Fields

    [SerializeField] private ActorInfo actorInfo;

    [Space, SerializeField] private UnityEvent<ActorHealthEventArgs> onDamaged;
    [SerializeField] private UnityEvent<ActorHealthEventArgs> onHealed;
    [SerializeField] public UnityEvent<Enemy> onDeath;

    #endregion

    #region Getters

    public GameObject GameObject => gameObject;

    public ActorInfo ActorInfo => actorInfo;

    public bool IsInvincible => actorInfo.InvincibilityTimer.CurrentValue > 0;

    #endregion

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
        if (IsInvincible && amount < 0)
            return;
        
        // Return if the amount is 0
        if (amount == 0)
            return;

        // Change the health value
        actorInfo.Health.ChangeValue(amount);

        // Create the event args
        var eventArgs = new ActorHealthEventArgs
        {
            Actor = this,
            Amount = amount,
            IsDamage = amount < 0
        };

        // Invoke the appropriate event
        if (amount < 0)
        {
            eventArgs.Amount = Mathf.Abs(amount);
            onDamaged?.Invoke(eventArgs);
        }
        else
            onHealed?.Invoke(eventArgs);
    }

    public void SetInvincibleForSeconds(float seconds)
    {
        // Set the invincibility timer to the specified seconds
        actorInfo.InvincibilityTimer.SetValue(seconds);
    }

    #endregion
}