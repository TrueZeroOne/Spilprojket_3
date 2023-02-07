using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPlatform : MonoBehaviour
{
    bool isGrabbed = false;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= 1)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (!isGrabbed)
                {
                    Debug.Log("HOLD THE FUCK ON!!!");
                    player.gameObject.transform.parent = gameObject.transform;
                    player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    player.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    isGrabbed = !isGrabbed;
                }
                else if (isGrabbed)
                {
                    Debug.Log("LET IT GO!!!");
                    player.gameObject.transform.parent = null;
                    player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    player.gameObject.GetComponent<PlayerMovement>().enabled = true;
                    gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    isGrabbed = !isGrabbed;
                }
            }
            if (isGrabbed)
            {
                gameObject.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, -1);
            }
        }
    }
}
