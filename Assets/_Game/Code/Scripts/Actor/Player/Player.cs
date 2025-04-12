using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IActor
{
    public static Player Instance { get; private set; }

    [SerializeField] private ActorInfo actorInfo;
    [SerializeField] private UnityEvent<Player> onDeath;

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
    public void ChangeHealth(float amount) => actorInfo.Health.ChangeValue(amount);

    #endregion
}