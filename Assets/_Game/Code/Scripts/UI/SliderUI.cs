using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderUI : MonoBehaviour
{
    [SerializeField] private RangedValueEvent rangedValueEvent;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image gradientImage;

    [SerializeField] private UnityEvent<RangedValue> onValueChanged;
    
    private Slider _slider;
    
    private void Awake()
    {
        // Get the slider component
        _slider = GetComponent<Slider>();
        
        // Connect the slider to the ranged value event
        rangedValueEvent.OnEventRaised.AddListener(onValueChanged.Invoke);
    }

    public void SetMinMaxCurrentValue(RangedValue value)
    {
        _slider.minValue = value.MinValue;
        _slider.maxValue = value.MaxValue;
        _slider.value = value.CurrentValue;
    }
    
    public void SetColor(RangedValue value)
    {
        // Get the color based on the current value
        // Set the color of the slider
        gradientImage.color = gradient.Evaluate(value.Percentage);
    }
}