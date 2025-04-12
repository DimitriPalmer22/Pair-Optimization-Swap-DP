using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable, BoxGroup]
public class RangedValue
{
    private const string TAB_GROUP = "Ranged Value";

    #region Serialized Fields

    [field: SerializeField, TabGroup(TAB_GROUP, "Values")]
    public float MinValue { get; private set; }

    [field: SerializeField, TabGroup(TAB_GROUP, "Values")]
    public float MaxValue { get; private set; }

    [field: SerializeField, TabGroup(TAB_GROUP, "Values")]
    public float CurrentValue { get; private set; }

    [field: SerializeField, TabGroup(TAB_GROUP, "Events")]
    public UnityEvent<RangedValue> OnValueChanged { get; private set; }

    #endregion

    [Button, TabGroup(TAB_GROUP, "Functions")]
    public bool SetValue(float value)
    {
        var previousValue = CurrentValue;

        CurrentValue = Mathf.Clamp(value, MinValue, MaxValue);

        // If the value didn't change, return false
        if (Mathf.Approximately(previousValue, CurrentValue))
            return false;

        // Invoke the event
        OnValueChanged?.Invoke(this);

        return true;
    }

    [Button, TabGroup(TAB_GROUP, "Functions")]
    public void ChangeValue(float amount) => SetValue(CurrentValue + amount);
}