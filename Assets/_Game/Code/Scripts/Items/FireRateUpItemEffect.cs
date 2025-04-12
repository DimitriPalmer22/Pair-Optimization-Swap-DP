using UnityEngine;

public class FireRateUpItemEffect : ItemEffect
{
    [SerializeField, Min(0)] private float fireRateIncrease = 0.1f;
    
    public override void Activate(Player player)
    {
        // Get the player attack component from the player
        var playerAttack = player.GetComponent<PlayerAttack>();
        
        // Decrease the cooldown value of the player's attack
        playerAttack.AttackCooldown.ChangeMaxValue(-fireRateIncrease);
    }
}