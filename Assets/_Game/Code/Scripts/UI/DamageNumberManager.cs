using Alchemy.Inspector;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField, Required] private Transform lookAtTransform;
    
    public void SpawnDamageNumber(ActorHealthEventArgs args)
    {
        // Instantiate the damage number prefab
        var damageNumber = Instantiate(damageNumberPrefab, transform);

        // Show the damage number
        damageNumber.ShowDamage(lookAtTransform, args);
    }
}