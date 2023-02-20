using UnityEngine;
using UnityEngine.InputSystem;

public class TinyBig : MonoBehaviour
{
    // Start is called before the first frame update
    //Size
    /*[SerializeField]
    private float pBigY = 3.5f;
    [SerializeField]
    private float pBigX = 1.5f;
    [SerializeField]
    private float pSmallY = 0.5f;
    [SerializeField]
    private float pSmallX = 1.0f;*/


    //private Vector3 v3Big;
    //private Vector3 v3Small;
    [SerializeField] private Vector3 currentSize;

    //Game Object
    private Transform pTF;
    private SpriteRenderer pSR;
    /*[SerializeField] private Color cBig;
    [SerializeField] private Color cSmall;*/
    [SerializeField] private Sprite spSmall;
    [SerializeField] private Sprite spBig;
    public bool isGrabbing = false;

    //KeyCode
    //public KeyCode keySize = KeyCode.C;

    //Bool
    public bool sizeBig = false;

    //RayCastHit2D
    private RaycastHit2D fitsUp;
    private RaycastHit2D fitsDown;

    //New Input System
    private PlayerInput playerInput;

    private PolygonCollider2D bigPC;
    private PolygonCollider2D smallPC;
    private Vector3 sizeDiffrence;
    private Animator playerAni;
    private AudioManager audioManager;
    private AudioSource audioSource;

    private Sprite currentSprite;

    private void Start()
    {
        playerAni = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        audioSource = audioManager.playerAudio;

        //v3Big = spBig.bounds.extents;
        //v3Small = spSmall.bounds.extents;
        pTF = gameObject.transform;
        pSR = GetComponent<SpriteRenderer>();
        //sizeBig = false;

        //New Input System
        playerInput = GetComponent<PlayerInput>();

        currentSprite = GetComponent<SpriteRenderer>().sprite;
        currentSize = currentSprite.bounds.extents;
        sizeDiffrence = spBig.bounds.extents - spSmall.bounds.extents;
    }

    private void Update()
    {
        ChangeCollider();

        fitsUp = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + currentSize.y+0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.up,10);
        fitsDown = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - currentSize.y-0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.down,10);
        //Debug.Log($"Fits Up = {fitsUp.distance}  Fits Down = {fitsDown.distance}");
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + currentSize.y + 0.05f), new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - currentSize.y - 0.05f), new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z));

        //Debug.Log(sizeDiffrence.y);

        if (playerInput.actions["ChangeSize"].triggered)//Key C
        {
            if(sizeBig == false)
            {
                if (!isGrabbing)
                {
                    if (fitsUp.distance > sizeDiffrence.y *2 +0.05f || fitsUp.distance == 0 && fitsUp.collider == null )
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
                    }
                    else
                    {
                        audioManager.PlayCannotGrow();
                    }
                }
                else if (isGrabbing)
                {
                    if (fitsDown.distance > sizeDiffrence.y *2 + 0.05f|| fitsDown.distance == 0 && fitsDown.collider == null)
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
                    }
                    else
                    {
                        audioManager.PlayCannotGrow();
                    }
                }

                //sizeBig = true;// Spilleren er STOR
                //SizeChange();
                /*if (sizeBig == true)
                {
                    pTF.localScale = new Vector3(pBigX, pBigY, 1);

                }
                pSR.color = cBig;
                pSR.sprite = spBig;*/

            }
            else if(sizeBig == true)
            {
                sizeBig = false;// Spilleren er LILLE
                SizeChange();
                /*if (sizeBig == false)
                {
                    pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
                }
                pSR.color = cSmall;
                pSR.sprite = spSmall;*/
            }
        }
    }

    private void ChangeCollider()
    {
        if(playerAni.GetCurrentAnimatorStateInfo(0).IsName("SizeUpGround") || playerAni.GetCurrentAnimatorStateInfo(0).IsName("SizeDownGround"))
            if (currentSprite != GetComponent<SpriteRenderer>().sprite)
            {
                PositionAfterSizeChange();
                Destroy(GetComponent<CapsuleCollider2D>());
                gameObject.AddComponent<CapsuleCollider2D>();
                currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentSize = currentSprite.bounds.extents;
            }
    }

    private void SizeChange()
    {

        if (sizeBig==true)
        {
            audioManager.PlayGrow();
            #region OutCommentedCode

            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pBigX,pBigY,1);
            //bigPC.enabled = true;
            //smallPC.enabled = false;
            //pSR.color = cBig;
            //currentSize = v3Big;

            #endregion
            playerAni.Play("SizeUpGround");
        }
        else if (sizeBig == false)
        {
            audioManager.PlayShrink();
            #region OutCommentedCode

            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
            //smallPC.enabled = true;
            //bigPC.enabled = false;
            //pSR.color = cSmall;
            //currentSize = v3Small;

            #endregion
            playerAni.Play("SizeDownGround");
        }
    }
    private void PositionAfterSizeChange()
    {
        if (isGrabbing)
        {
            if (sizeBig)
            {
                pTF.position = new Vector3(pTF.position.x, pTF.position.y - sizeDiffrence.y, pTF.position.z);
            }
            else if (!sizeBig)
            {
                pTF.position = new Vector3(pTF.position.x, pTF.position.y + sizeDiffrence.y, pTF.position.z);
            }
        }
        else if (!isGrabbing)
        {
            if (sizeBig)
            {
                if (!GetComponent<PlayerMovement>().grounded)
                {
                    return;
                }
                else
                {
                    pTF.position = new Vector3(pTF.position.x, pTF.position.y + sizeDiffrence.y, pTF.position.z);
                }
            }
            else if (!sizeBig)
            {
                if (!GetComponent<PlayerMovement>().grounded)
                {
                    if (fitsDown.distance > sizeDiffrence.y && fitsUp.distance > sizeDiffrence.y)
                    {
                        return;
                    }
                    else
                    {
                        pTF.position = new Vector3(pTF.position.x, pTF.position.y - sizeDiffrence.y, pTF.position.z);
                    }
                }
                else
                {
                    pTF.position = new Vector3(pTF.position.x, pTF.position.y - sizeDiffrence.y, pTF.position.z);
                }
            }
        }
    }
}
