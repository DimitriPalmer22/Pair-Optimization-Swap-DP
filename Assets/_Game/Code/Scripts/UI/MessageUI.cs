using System;
using System.Collections;
using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MessageUI : MonoBehaviour
{
    [SerializeField, Required] private LoggerScriptableObject loggerEvent;

    [SerializeField, Required] private TMP_Text messageText;
    [SerializeField, Required] private CanvasGroup canvasGroup;

    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private UnityEvent<string> onMessageReceived;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        // Connect the logger event to the message text
        loggerEvent.onBroadcastLog.AddListener(onMessageReceived.Invoke);

        // Set the initial alpha value of the canvas group
        canvasGroup.alpha = fadeCurve.Evaluate(0);
    }

    public void ChangeText(string text)
    {
        // Change the message text
        messageText.text = text;
    }

    public void StartFadeCoroutine()
    {
        // Stop the previous fade coroutine if it's running
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        // Start a new fade coroutine
        _fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        var startTime = Time.time;

        var fadeLength = fadeCurve.keys[^1].time;

        while (Time.time < startTime + fadeLength)
        {
            // Calculate the alpha value based on the animation curve
            var alpha = fadeCurve.Evaluate(Time.time - startTime);

            // Set the alpha value of the canvas group
            canvasGroup.alpha = alpha;

            yield return null;
        }

        // Set the alpha value to 0
        canvasGroup.alpha = fadeCurve.Evaluate(fadeLength);
    }
}