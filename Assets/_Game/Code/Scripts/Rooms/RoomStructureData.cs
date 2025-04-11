using System;
using Alchemy.Inspector;
using UnityEngine;

[Serializable, BoxGroup]
public class RoomStructureData
{
    [field: SerializeField, Required] public Transform leftDoorTransform { get; private set; }
    [field: SerializeField, Required] public Transform rightDoorTransform { get; private set; }
    [field: SerializeField, Required] public Transform itemSpawnLocation { get; private set; }
    
    [field: SerializeField, Required] public Transform[] enemySpawnLocations { get; private set; }
}