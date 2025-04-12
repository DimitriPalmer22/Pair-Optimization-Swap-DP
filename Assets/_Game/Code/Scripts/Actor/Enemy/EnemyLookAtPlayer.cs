using System;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    private void Update()
    {
        // Return if the player instance is null
        if (Player.Instance == null)
            return;

        // Get the direction to the player
        var directionToPlayer = Player.Instance.transform.position - transform.position;
        directionToPlayer.y = 0;
        directionToPlayer.Normalize();

        // Rotate the enemy towards the player
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(directionToPlayer),
            Time.deltaTime * rotationSpeed
        );
    }
}