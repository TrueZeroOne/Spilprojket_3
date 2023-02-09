using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int buildIndex;
    public static int nextScene = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {              
            if(collision.gameObject.tag == "Player")
            {
            nextScene++;
                SceneManager.LoadScene(nextScene-1);
            }
        
        
    }
}
