using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPrefabList", menuName = "ScriptableObjects/ItemPrefabList", order = 1)]
public class ItemPrefabList : ScriptableObject
{
    [SerializeField] private Item[] itemPrefabs;

    public IReadOnlyList<Item> ItemPrefabs => itemPrefabs;
}