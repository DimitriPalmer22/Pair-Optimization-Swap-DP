using UnityEngine;

public interface IActor
{
    public GameObject GameObject { get; }
    
    public ActorInfo ActorInfo { get; }
    
    public void ChangeHealth(float amount);
    public void CheckForDeath(RangedValue health);

    public void SetInvincibleForSeconds(float seconds);
}