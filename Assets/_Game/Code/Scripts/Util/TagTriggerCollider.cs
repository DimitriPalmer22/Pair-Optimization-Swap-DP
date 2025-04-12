using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TagTriggerCollider : MonoBehaviour
{
    [SerializeField] private string[] acceptedTags;
    [SerializeField] private bool activateOnce;

    [SerializeField] private UnityEvent onEnter;

    private bool _hasActivated;

    private void OnTriggerEnter(Collider other)
    {
        // Return if the trigger has already activated once
        if (activateOnce && _hasActivated)
            return;

        // Check if the other object has a tag that is in the accepted tags
        if (acceptedTags != null && !acceptedTags.Contains(other.gameObject.tag))
            return;

        // Set the trigger to activated (if activateOnce is true)
        if (activateOnce)
            _hasActivated = true;

        // Invoke the event
        onEnter?.Invoke();
    }
}