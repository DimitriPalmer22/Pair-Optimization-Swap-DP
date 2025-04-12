using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    public static int RoomCount { get; private set; }

    #region Serialized Fields

    [SerializeField] private RoomStructureData roomStructureData;

    [SerializeField, Range(0, 1)] private float itemSpawnChance = 0.5f;
    [SerializeField, Required] private ItemPrefabList itemPrefabList;

    [SerializeField, Required] private RoomPrefabList nextRooms;

    [SerializeField] private UnityEvent<Room> onRoomStarted;
    [SerializeField] private UnityEvent<Room> onRoomCleared;

    #endregion

    #region Private Fields

    private int _remainingEnemyCount;
    private bool _isRoomCleared;
    private bool _isNextRoomSpawned;
    private Room _previousRoom;

    private readonly HashSet<Enemy> _managedEnemies = new();

    #endregion

    private void Awake()
    {
        // Open the doors
        OpenDoors();
    }

    [Button]
    public void StartRoom()
    {
        // Return if the room is already cleared
        if (_isRoomCleared)
        {
            Debug.LogWarning("Room is already cleared. Cannot start again.", this);
            return;
        }

        // If the previous room is not null, destroy it
        if (_previousRoom != null)
        {
            Destroy(_previousRoom.gameObject);
            _previousRoom = null;
        }

        // Increment the room count
        RoomCount++;

        // Invoke the room started event
        onRoomStarted.Invoke(this);

        // Spawn the enemies
        SpawnEnemies();
    }

    public void BroadcastRoomCount(LoggerScriptableObject logger)
    {
        logger.BroadcastLog($"Room #{RoomCount}");
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

        // Based on the item spawn chance, spawn an item
        if (UnityEngine.Random.value <= itemSpawnChance)
        {
            // Get a random item prefab from the list
            var itemPrefabs = itemPrefabList.ItemPrefabs;
            var randomItemPrefab = itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Count)];
            var item = Instantiate(randomItemPrefab, roomStructureData.itemSpawnLocation.position, Quaternion.identity);

            // Connect the onItemPicked event to the room cleared event
            item.OnPickUp.AddListener(_ => onRoomCleared?.Invoke(this));
        }

        // If the item is not spawned, just invoke the room cleared event
        else
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

    private void OnEnemyDeath(Enemy enemy)
    {
        // Remove this enemy from the managed enemies
        _managedEnemies.Remove(enemy);

        // Decrement the remaining enemy count
        _remainingEnemyCount--;

        // If the remaining enemy count is 0, clear the room
        if (_remainingEnemyCount <= 0)
            ClearRoom();
    }

    [Button]
    public void SpawnNextRoom()
    {
        // Return if the next room is already spawned
        if (_isNextRoomSpawned)
        {
            Debug.LogWarning("Next room is already spawned. Cannot spawn again.", this);
            return;
        }

        // Set the next room as spawned
        _isNextRoomSpawned = true;

        // Instantiate the next room prefab
        // Get a random room prefab from the list
        var roomPrefabs = nextRooms.RoomPrefabs;
        var randomRoomPrefab = roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)];
        var nextRoom = Instantiate(randomRoomPrefab, Vector3.zero, Quaternion.identity);

        // Get the position offset from the next room's position and its left door
        var nextRoomOffset = nextRoom.transform.position - nextRoom.roomStructureData.leftDoorTransform.position;

        // Set the next room's position to be right next to this room's right door
        nextRoom.transform.position = roomStructureData.rightDoorTransform.position + nextRoomOffset;

        // Set the next room's previous room to this room
        nextRoom._previousRoom = this;
    }

    [Button]
    public void CloseDoors()
    {
        roomStructureData.leftDoor.gameObject.SetActive(true);
        roomStructureData.rightDoor.gameObject.SetActive(true);
    }

    [Button]
    public void OpenDoors()
    {
        roomStructureData.leftDoor.gameObject.SetActive(false);
        roomStructureData.rightDoor.gameObject.SetActive(false);
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