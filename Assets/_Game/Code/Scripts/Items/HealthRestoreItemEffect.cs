using UnityEngine;

public class HealthRestoreItemEffect : ItemEffect
{
    [SerializeField] private float restoreAmount = 25f;
    
    public override void Activate(Player player)
    {
        // Restore health to the player
        player.ChangeHealth(restoreAmount);
    }
}