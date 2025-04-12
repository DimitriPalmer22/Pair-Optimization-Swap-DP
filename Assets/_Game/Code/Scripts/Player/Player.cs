using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;

    [SerializeField] private UnityEvent<Player> onDeath;

    #region Public Functions
    
    public void CheckForDeath(RangedValue health)
    {
        if (health.CurrentValue > 0) 
            return;
        
        onDeath?.Invoke(this);
    }
    
    public void LogPlayerHealth()
    {
        Debug.Log($"Player Health: {playerInfo.Health.CurrentValue}", this);
    }
    
    public void LogPlayerDodgeCooldown()
    {
        Debug.Log($"Player Dodge Cooldown: {playerInfo.DodgeCooldown.CurrentValue}", this);
    }
    
    #endregion
}