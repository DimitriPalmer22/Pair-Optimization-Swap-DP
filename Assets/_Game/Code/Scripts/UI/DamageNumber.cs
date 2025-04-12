using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    #region Serialized Fields

    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    [field: SerializeField] public TMP_Text Text { get; private set; }

    [Space, SerializeField] private Color healColor;
    [SerializeField] private Color damageColor;

    [Space, SerializeField] private Vector3 randomPositionOffset;
    [SerializeField] private float yOffset;

    [SerializeField] private AnimationCurve fadeOutCurve;

    #endregion

    private Coroutine _fadeCoroutine;

    public void ShowDamage(Transform lookAt, ActorHealthEventArgs args)
    {
        // Set the text color based on whether it's damage or healing
        Text.color = args.IsDamage ? damageColor : healColor;

        // Set the text value
        Text.text = $"{(args.IsDamage ? "-" : "+")}{args.Amount}";

        var actorPosition = args.Actor.GameObject.transform.position;

        var randomOffset = new Vector3(
            Random.Range(-randomPositionOffset.x, randomPositionOffset.x),
            Random.Range(-randomPositionOffset.y, randomPositionOffset.y),
            Random.Range(-randomPositionOffset.z, randomPositionOffset.z)
        );

        // Set the position
        transform.position = new Vector3(actorPosition.x, actorPosition.y + yOffset, actorPosition.z) + randomOffset;

        // Start the fade coroutine
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeCoroutine(lookAt));
    }

    private IEnumerator FadeCoroutine(Transform lookAt)
    {
        var startTime = Time.time;
        var duration = fadeOutCurve.keys[^1].time;

        // Set the alpha based on the animation curve
        while (Time.time < startTime + duration)
        {
            // Look at the target
            transform.forward = -(lookAt.position - transform.position);

            // Calculate the alpha value based on the curve
            var alpha = fadeOutCurve.Evaluate(Time.time - startTime);
            CanvasGroup.alpha = alpha;

            // Wait for the next frame
            yield return null;
        }

        var alphaValue = fadeOutCurve.Evaluate(duration);

        // Destroy the game object
        Destroy(gameObject);
    }
}