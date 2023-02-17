using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject pauseMenu;

    PlayerInput playerInput;
    [SerializeField] private TMP_Text textMP;
    [SerializeField] private string input;
    InputAction action;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        action = playerInput.actions[input];
        textMP.text = action.bindings[0].ToDisplayString();
    }
    public void Update()
    {
        if (action.triggered)
        {
            isPaused = !isPaused;
            Debug.Log("Escape was pressed");
        }
        if (!isPaused)
        {
            pauseMenu.SetActive(false);
            gameCanvas.SetActive(true);          
        }
        else
        {
            pauseMenu.SetActive(true);
            gameCanvas.SetActive(false);
        }
    }
}
