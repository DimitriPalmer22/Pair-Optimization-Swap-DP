using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventScriptableObject<T> : ScriptableObject
{
    [field: SerializeField] public UnityEvent<T> OnEventRaised { get; protected set; }

    [Button]
    public void RaiseEvent(T value) => OnEventRaised?.Invoke(value);
}