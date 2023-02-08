using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPlatform : MonoBehaviour
{
    bool isGrabbed = false;
    GameObject player;
    Vector2 speed;
    public int moveSpeed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    void Update()
    {
        var dotProduct = Vector3.Dot(Vector3.down, (player.transform.position - transform.position).normalized);
        Debug.Log(dotProduct);
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isGrabbed)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= player.transform.localScale.y + transform.localScale.y && dotProduct >= 0.9)
                {
                    Debug.Log("HOLD THE FUCK ON!!!");
                    player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1);
                    player.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    isGrabbed = !isGrabbed;
                    player.GetComponent<TinyBig>().isGrabbing = true;
                }
            }
            else if (isGrabbed)
            {
                Debug.Log("LET IT GO!!!");
                player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1);
                player.gameObject.GetComponent<PlayerMovement>().enabled = true;
                gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                isGrabbed = !isGrabbed;
                player.GetComponent<TinyBig>().isGrabbing = false;
            }
            
        }
        if (isGrabbed)
        {
            if (!TinyBig.sizeBig)
            {
                speed = new Vector2(0, -1);
                speed *= moveSpeed;
            }
            else
            {
                speed = new Vector2(0, 0);
                speed *= moveSpeed;
            }
            gameObject.GetComponentInParent<Rigidbody2D>().velocity = speed;
            gameObject.GetComponentInParent<Rigidbody2D>().AddRelativeForce(speed);
        }
    }
}
