using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class helpText : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private string input;
    [SerializeField] private TMP_Text textMP;
    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        InputAction action = playerInput.actions[input];
        textMP.text = action.bindings[0].ToDisplayString();
    }
}
