using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using System;

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

    [SerializeField] AudioClip cantSize;
    [SerializeField] AudioClip changeSize;
    [SerializeField] AudioClip rChangeSize;

    private Sprite currentSprite;
    private static readonly int SizeAnimID = Animator.StringToHash("Size");

    private void Start()
    {
        playerAni = GetComponent<Animator>();
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

    // Update is called once per frame
    private void Update()
    {
        ChangeCollider();

        fitsUp = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + currentSize.y+0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.up,10);
        fitsDown= Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - currentSize.y-0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.down,10);
        Debug.Log("Fits  Up = "+fitsUp.distance + "  Fits Down = "+fitsDown.distance);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + currentSize.y + 0.05f), new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - currentSize.y - 0.05f), new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z));

        if (playerInput.actions["ChangeSize"].triggered)//Key C
        {
            if(sizeBig == false)
            {
                if (!isGrabbing)
                {
                    if (fitsUp.distance > sizeDiffrence.y +0.05f || fitsUp.distance == 0 && fitsUp.collider == null )
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
                    }
                    else
                    {
                        GetComponent<AudioSource>().clip = cantSize;
                        GetComponent<AudioSource>().Play();
                    }
                }
                else if (isGrabbing)
                {
                    if (fitsDown.distance > sizeDiffrence.y  + 0.05f|| fitsDown.distance == 0 && fitsDown.collider == null)
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
                    }
                    else
                    {
                        GetComponent<AudioSource>().clip = cantSize;
                        GetComponent<AudioSource>().Play();
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
            GetComponent<AudioSource>().clip = changeSize;
            GetComponent<AudioSource>().Play();
            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pBigX,pBigY,1);
            //bigPC.enabled = true;
            //smallPC.enabled = false;
            //pSR.color = cBig;
            playerAni.Play("SizeUpGround");
            //currentSize = v3Big;
        }
        else if (sizeBig == false)
        {
            GetComponent<AudioSource>().clip = rChangeSize;
            GetComponent<AudioSource>().Play();
            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
            //smallPC.enabled = true;
            //bigPC.enabled = false;
            //pSR.color = cSmall;
            playerAni.Play("SizeDownGround");
            //currentSize = v3Small;
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
