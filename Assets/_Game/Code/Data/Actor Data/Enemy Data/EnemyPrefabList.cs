using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPrefabList", menuName = "ScriptableObjects/EnemyPrefabList")]
public class EnemyPrefabList : ScriptableObject
{
    [SerializeField] private Enemy[] enemyPrefabs;
    
    public IReadOnlyList<Enemy> EnemyPrefabs => enemyPrefabs;
}