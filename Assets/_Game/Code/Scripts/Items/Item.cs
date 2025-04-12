using Alchemy.Inspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemEffect[] itemEffects;

    [Button]
    public void PickUp(Player player)
    {
        // Activate item effects
        ActivateItemEffects(player);

        // Destroy the item game object
        Destroy(gameObject);
    }

    private void ActivateItemEffects(Player player)
    {
        // Iterate through all item effects and activate them
        foreach (var effect in itemEffects)
            effect.Activate(player);
    }
}