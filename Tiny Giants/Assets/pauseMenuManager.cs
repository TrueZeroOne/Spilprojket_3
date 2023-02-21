using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pauseMenuManager : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private string input;
    private InputAction action;
    public bool isPaused;

    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject keyBindsCanvas;
    [SerializeField] GameObject keyBindsManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameCanvas;

    private void Start()
    {
        gameCanvas = GameObject.Find("UICanvas");
        pauseMenu.SetActive(false);
        playerInput =GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        action = playerInput.actions[input];
    }
    public void Update()
    {
        if (action.triggered)
        {
            if (keyBindsCanvas.activeInHierarchy)
            {
                keyBindsCanvas.SetActive(false);
                keyBindsManager.SetActive(false);
                settingsCanvas.SetActive(true);
            }
            else if (settingsCanvas.activeInHierarchy)
            {
                settingsCanvas.SetActive(false);
                pauseMenu.SetActive(true);
            }
            else if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                gameCanvas.SetActive(true);
            }
            else if (gameCanvas.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                gameCanvas.SetActive(false);
            }
        }
    }
    public void Settings()
    {
        settingsCanvas.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void Keybinds()
    {
        keyBindsCanvas.SetActive(true);
        keyBindsManager.SetActive(true);
        settingsCanvas.SetActive(false);
    }
}
