using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable, FoldoutGroup]
public class PlayerInfo
{
    [field: SerializeField] public RangedValue Health { get; private set; }
    [field: SerializeField] public RangedValue DodgeCooldown { get; private set; }
}