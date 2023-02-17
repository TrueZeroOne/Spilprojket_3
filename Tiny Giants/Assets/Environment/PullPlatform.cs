using UnityEngine;
using UnityEngine.InputSystem;

public class PullPlatform : MonoBehaviour
{
    public bool isGrabbed = false;
    private GameObject player;
    private Vector2 speed;
    public int moveSpeed;
    private TinyBig tinyBig;
    private PlayerInput playerInput;
    private float playerHeight;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tinyBig = player.GetComponent<TinyBig>();
        playerInput = player.GetComponent<PlayerInput>();
        playerHeight = player.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
    }

    private void Update()
    {
        playerHeight = player.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        Vector2 nearestToPoint = player.GetComponent<CapsuleCollider2D>().ClosestPoint(transform.position);
        
        if (playerInput.actions["Pull"].triggered)
        {
            if (!isGrabbed)
            {
                if (Vector2.Distance(nearestToPoint, transform.position) <= GetComponent<SpriteRenderer>().sprite.bounds.extents.y+0.5f)
                {
                    player.GetComponent<Rigidbody2D>().gravityScale = 0;
                    player.GetComponent<PlayerMovement>().grabbingPlatform = true;
                    gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    isGrabbed = !isGrabbed;
                    player.GetComponent<TinyBig>().isGrabbing = true;
                }
            }
            else if (isGrabbed)
            {
                Debug.Log("LET IT GO!!!");
                player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                player.gameObject.GetComponent<PlayerMovement>().grabbingPlatform = false;
                gameObject.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                isGrabbed = !isGrabbed;
                player.GetComponent<TinyBig>().isGrabbing = false;
            }
            
        }
        if (isGrabbed)
        {
            if (!tinyBig.sizeBig)
            {
                speed = new Vector2(0, -1);
                speed *= moveSpeed;
            }
            else
            {
                speed = new Vector2(0, 0);
                speed *= moveSpeed;
            }
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                gameObject.GetComponentInParent<Rigidbody2D>().velocity = speed;
                gameObject.GetComponentInParent<Rigidbody2D>().AddRelativeForce(speed);
                player.GetComponent<Rigidbody2D>().velocity = speed;
                player.GetComponent<Rigidbody2D>().AddRelativeForce(speed);
            }
            else
            {
                gameObject.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<SpriteRenderer>().sprite.bounds.extents.y + 0.4f);
    }
}
