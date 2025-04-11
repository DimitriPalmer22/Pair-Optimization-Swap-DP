using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomStructureData roomStructureData;

    private void OnDrawGizmos()
    {
        const float sphereRadius = .5f;
        
        // Draw all the doors
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(roomStructureData.leftDoorTransform.position, sphereRadius);
        Gizmos.DrawSphere(roomStructureData.rightDoorTransform.position, sphereRadius);
        
        // Draw the item spawn location
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(roomStructureData.itemSpawnLocation.position, sphereRadius);
        
        // Draw the enemy spawn locations
        Gizmos.color = Color.red;
        foreach (var location in roomStructureData.enemySpawnLocations)
            Gizmos.DrawSphere(location.position, sphereRadius);
    }
}