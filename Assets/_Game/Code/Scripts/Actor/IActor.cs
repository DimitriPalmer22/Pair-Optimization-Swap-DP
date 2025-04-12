public interface IActor
{
    public ActorInfo ActorInfo { get; }
    
    public void ChangeHealth(float amount);
    public void CheckForDeath(RangedValue health);

    public void SetInvincibleForSeconds(float seconds);
}