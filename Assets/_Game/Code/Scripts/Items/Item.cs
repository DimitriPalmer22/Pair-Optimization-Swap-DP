using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemEffect[] itemEffects;

    [field: SerializeField] public UnityEvent<Item> OnPickUp { get; private set; }

    [Button]
    public void PickUp(Player player)
    {
        // Activate item effects
        ActivateItemEffects(player);

        // Invoke the OnPickUp event
        OnPickUp?.Invoke(this);
        
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