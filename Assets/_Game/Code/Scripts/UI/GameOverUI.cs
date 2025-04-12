using System;
using Alchemy.Inspector;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField, Required] private SceneReference sceneToLoad;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.Restart.performed += ReloadSceneOnRestart;
    }

    private void ReloadSceneOnRestart(InputAction.CallbackContext obj)
    {
        ReloadScene();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void ReloadScene()
    {
        // Check if the scene to load is set
        if (sceneToLoad == null)
        {
            Debug.LogError("Scene to load is not set.");
            return;
        }

        // Reload the scene
        SceneManager.LoadScene(sceneToLoad.Name);
    }
}