using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomPrefabList", menuName = "ScriptableObjects/RoomPrefabList", order = 1)]
public class RoomPrefabList : ScriptableObject
{
    [SerializeField] private Room[] roomPrefabs;
    
    public IReadOnlyList<Room> RoomPrefabs => roomPrefabs;
}