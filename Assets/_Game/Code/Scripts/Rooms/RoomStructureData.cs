using System;
using Alchemy.Inspector;
using UnityEngine;

[Serializable, BoxGroup]
public class RoomStructureData
{
    [field: SerializeField, Required] public Transform leftDoorTransform { get; private set; }
    [field: SerializeField, Required] public Transform rightDoorTransform { get; private set; }
    [field: SerializeField, Required] public Transform itemSpawnLocation { get; private set; }

    [field: SerializeField, Required] public Transform leftDoor { get; private set; }
    [field: SerializeField, Required] public Transform rightDoor { get; private set; }

    [field: SerializeField, Required] public RoomEnemySpawner[] enemySpawners { get; private set; }
}