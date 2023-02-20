using UnityEngine;
using Cinemachine;

public class CMSwitcher : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public Collider2D boundShape;
    public Bounds bounds;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        boundShape = transform.GetChild(0).GetComponent<CinemachineConfiner2D>().m_BoundingShape2D;
    }
    void Start()
    {
        boundShape = transform.GetChild(0).GetComponent<CinemachineConfiner2D>().m_BoundingShape2D;
        boundShape.enabled = true;
        bounds = boundShape.bounds;
        boundShape.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounds.Contains(player.transform.position))
        {
            //Debug.Log("Inside");
            animator.Play("Inside");
        }
        else if(!bounds.Contains(player.transform.position))
        {
            //Debug.Log("Outside");
            animator.Play("Outside");
        }
    }
}
