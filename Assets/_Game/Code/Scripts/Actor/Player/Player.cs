using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private ActorInfo actorInfo;

    [SerializeField] private UnityEvent<Player> onDeath;

    public ActorInfo ActorInfo => actorInfo;

    private void Awake()
    {
        // Set the initial health value
        actorInfo.Health.SetValue(actorInfo.Health.MaxValue);
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