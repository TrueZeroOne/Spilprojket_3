using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject pauseMenu;

    private PlayerInput playerInput;
    [SerializeField] private string input;
    private InputAction action;
    public bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        action = playerInput.actions[input];
    }
    public void Update()
    {
        if (action.triggered)
        {
            isPaused = !isPaused;
            //Debug.Log("Escape was pressed");
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
