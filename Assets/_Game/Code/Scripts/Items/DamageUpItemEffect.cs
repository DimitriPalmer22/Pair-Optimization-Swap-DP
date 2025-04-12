using UnityEngine;

public class DamageUpItemEffect : ItemEffect
{
    [SerializeField, Min(0)] private float damageIncrease = 5f;


    public override void Activate(Player player)
    {
        // Get the player attack component from the player
        var playerAttack = player.GetComponent<PlayerAttack>();

        // Decrease the cooldown value of the player's attack
        playerAttack.DamageAdded.ChangeValue(damageIncrease);
    }
}