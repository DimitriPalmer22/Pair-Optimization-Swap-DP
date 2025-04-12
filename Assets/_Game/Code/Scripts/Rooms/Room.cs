using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomStructureData roomStructureData;

    [SerializeField] private UnityEvent<Room> onRoomStarted;
    [SerializeField] private UnityEvent<Room> onRoomCleared;

    #region Private Fields

    private int _remainingEnemyCount;
    private bool _isRoomCleared;
    
    private readonly HashSet<Enemy> _managedEnemies = new();

    #endregion

    [Button]
    public void StartRoom()
    {
        // Return if the room is already cleared
        if (_isRoomCleared)
        {
            Debug.LogWarning("Room is already cleared. Cannot start again.", this);
            return;
        }
        
        // Spawn the enemies
        SpawnEnemies();
    }

    private void ClearRoom()
    {
        // Return if the room is already cleared
        if (_isRoomCleared)
        {
            Debug.LogWarning("Room is already cleared. Cannot clear again.", this);
            return;
        }

        // Set the room as cleared
        _isRoomCleared = true;

        // Invoke the room cleared event
        onRoomCleared?.Invoke(this);
    }

    [Button]
    private void SpawnEnemies()
    {
        // Return if the structure data's enemy spawners are null or empty
        if (roomStructureData.enemySpawners == null || roomStructureData.enemySpawners.Length == 0)
        {
            Debug.LogWarning("No enemy spawners found in the room structure data.", this);
            return;
        }
        
        // Return if the room is already cleared
        if (_isRoomCleared)
        {
            Debug.LogWarning("Room is already cleared. Cannot spawn enemies.", this);
            return;
        }

        foreach (var spawner in roomStructureData.enemySpawners)
        {
            // Get a random enemy prefab from the spawner
            var prefabList = spawner.EnemyPrefabList.EnemyPrefabs;
            var randomEnemyPrefab = prefabList[UnityEngine.Random.Range(0, prefabList.Count)];

            // Spawn the enemy
            SpawnEnemy(randomEnemyPrefab, spawner);
        }
    }

    private void SpawnEnemy(Enemy prefab, RoomEnemySpawner spawner)
    {
        // Return if the room is cleared
        if (_isRoomCleared)
        {
            Debug.LogWarning("Room is already cleared. Cannot spawn more enemies.", this);
            return;
        }

        // Instantiate the random enemy prefab at the spawner's position and rotation
        var enemy = Instantiate(prefab, spawner.transform.position, spawner.transform.rotation);

        // Manage the enemy
        _managedEnemies.Add(enemy);
        
        // Subscribe to the enemy's death event
        enemy.onDeath.AddListener(OnEnemyDeath);
        
        // Increment the remaining enemy count
        _remainingEnemyCount++;
    }

    private void OnEnemyDeath(Enemy arg0)
    {
        // Remove this enemy from the managed enemies
        _managedEnemies.Remove(arg0);
        
        // Decrement the remaining enemy count
        _remainingEnemyCount--;
        
        Debug.Log($"Remaining enemies: {_remainingEnemyCount}", this);
        
        // If the remaining enemy count is 0, clear the room
        if (_remainingEnemyCount <= 0)
            ClearRoom();
    }

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
        foreach (var location in roomStructureData.enemySpawners)
            Gizmos.DrawSphere(location.transform.position, sphereRadius);
    }
}