using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInvincibilityFlash : MonoBehaviour
{
    [SerializeField] private Renderer[] renderersToFlash;
    [SerializeField] private float flashDuration = 0.125f;

    private Player _player;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        // Get the Player component
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        // Start the flash coroutine if it is not already active
        if (_flashCoroutine == null && Player.Instance.IsInvincible)
            _flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // variable to keep track of the current state of the renderers
        var currentlyOn = true;

        while (_player.IsInvincible)
        {
            // Flash the renderers
            SetRenderersEnabled(!currentlyOn);
            currentlyOn = !currentlyOn;

            // Wait for the flash duration
            yield return new WaitForSeconds(flashDuration);
        }

        // Set the renderers to enabled state
        SetRenderersEnabled(true);

        // Set the flash coroutine to null
        _flashCoroutine = null;
    }

    private void SetRenderersEnabled(bool isEnabled)
    {
        foreach (var cRenderer in renderersToFlash)
            cRenderer.enabled = isEnabled;
    }
}