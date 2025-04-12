using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerCollider : MonoBehaviour
{
    [SerializeField] private bool activateOnce;

    [SerializeField] private UnityEvent<Player> onEnter;

    private bool _hasActivated;

    private void OnTriggerEnter(Collider other)
    {
        // Return if the trigger has already activated once
        if (activateOnce && _hasActivated)
            return;

        // Set the trigger to activated (if activateOnce is true)
        if (activateOnce)
            _hasActivated = true;

        // Check if the other object has a player component
        if (other.gameObject.TryGetComponent(out Player player) == false)
            return;

        // Invoke the event
        onEnter?.Invoke(player);
    }
}