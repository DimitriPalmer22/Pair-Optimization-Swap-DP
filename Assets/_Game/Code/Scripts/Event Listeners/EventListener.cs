using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<TScriptableObject, TArgs> : MonoBehaviour
    where TScriptableObject : EventScriptableObject<TArgs>
{
    [SerializeField, Required] protected TScriptableObject eventScriptableObject;
    
    [SerializeField] protected UnityEvent<TArgs> onEventRaised;

    protected void Awake()
    {
        // Subscribe to the event
        eventScriptableObject.OnEventRaised.AddListener(onEventRaised.Invoke);
        
        // Call the custom awake method
        CustomAwake();
    }

    private void OnDestroy()
    {
        // Remove the listener when destroyed
        eventScriptableObject.OnEventRaised.RemoveListener(onEventRaised.Invoke);
        
        // Call the custom destroy method
        CustomDestroy();
    }

    protected abstract void CustomDestroy();

    protected abstract void CustomAwake();

    [Button]
    public void RaiseEvent(TArgs args)
    {
        if (eventScriptableObject != null)
            eventScriptableObject.RaiseEvent(args);
        else
            Debug.LogWarning("Event scriptable object is not assigned.");
    }
}