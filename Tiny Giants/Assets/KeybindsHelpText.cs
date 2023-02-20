using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindsHelpText : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject keybindHelpUI;
    [SerializeField] private TMP_Text textMP;
    [SerializeField] private string input;
    [SerializeField] private int bindingIndex;
    private InputAction action;
    private InputBinding binding;

    // Start is called before the first frame update
    private void Start()
    {
        keybindHelpUI.SetActive(false);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        action = playerInput.actions[input];
        binding = action.bindings[0];
        textMP.text = binding.isComposite ? action.GetBindingDisplayString(bindingIndex: bindingIndex) : binding.ToDisplayString();
    }

    // Update is called once per frame                     
    private void Update()
    {
        if (playerInput.actions["keybindHelp"].ReadValue<float>() == 1)
        {
            //Debug.Log("Tab is down");
            keybindHelpUI.SetActive(true);
        }
        else if (playerInput.actions["keybindHelp"].ReadValue<float>() == 0)
        {
            keybindHelpUI.SetActive(false);
        }
        
    }
}
