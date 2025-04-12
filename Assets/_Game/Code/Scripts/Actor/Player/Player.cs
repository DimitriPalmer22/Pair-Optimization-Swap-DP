using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IActor
{
    public static Player Instance { get; private set; }

    #region Serialized Fields

    [SerializeField] private ActorInfo actorInfo;

    [Space, SerializeField] private UnityEvent<ActorHealthEventArgs> onDamaged;
    [SerializeField] private UnityEvent<ActorHealthEventArgs> onHealed;
    [SerializeField] private UnityEvent<Player> onDeath;

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

    private void OnEnable()
    {
        // Set the instance to this
        Instance = this;
    }

    private void OnDisable()
    {
        // Unset the instance
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        // Tick the invincibility timer
        actorInfo.InvincibilityTimer.ChangeValue(-Time.deltaTime);
    }

    #region Public Functions

    public void CheckForDeath(RangedValue health)
    {
        if (health.CurrentValue > 0)
            return;

        onDeath?.Invoke(this);
    }

    public void LogPlayerHealth()
    {
        Debug.Log($"Player Health: {actorInfo.Health.CurrentValue}", this);
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