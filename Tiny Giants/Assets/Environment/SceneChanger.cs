using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using System;

public class SceneChanger : MonoBehaviour
{
    public int buildIndex;
    public GameObject pressEUI;
    PlayerInput playerInput;
    [SerializeField] private string input;
    InputAction action;
    public bool isActive;
    public void Start()
    {
        pressEUI.SetActive(false);
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        action = playerInput.actions[input];
    }

    private void Update()
    {
        if (isActive)
        {
            if (action.triggered)
            {
                Debug.Log("E was pressed");
                ChangeScene();
                
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStayOrEnter(collision);             
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerStayOrEnter(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isActive = false;
        pressEUI.SetActive(false);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
    private void OnTriggerStayOrEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            pressEUI.SetActive(true);
        }
        
    }
}
