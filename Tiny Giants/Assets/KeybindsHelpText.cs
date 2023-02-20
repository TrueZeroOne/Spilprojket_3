using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindsHelpText : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject keybindHelpUI;
    [SerializeField] private TMP_Text textMP;
    [SerializeField] private string input;
    [SerializeField] private int actionNr;
    [SerializeField] private InputActionReference IAR;


    // Start is called before the first frame update
    private void Start()
    {
        keybindHelpUI.SetActive(false);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        InputAction action = playerInput.actions[input];
        if (action.bindings[0].isComposite)
        {
            Debug.Log("Composite");
        }
        textMP.text = action.bindings[0].ToDisplayString();
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerInput.actions["keybindHelp"].ReadValue<float>() == 1)
        {
            Debug.Log("Tab was pressed");
            keybindHelpUI.SetActive(true);
        }
        else if (playerInput.actions["keybindHelp"].ReadValue<float>() == 0)
        {
            keybindHelpUI.SetActive(false);
        }
        
    }
}
