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
    [SerializeField]
    private float pBigY = 3.5f;
    [SerializeField]
    private float pBigX = 1.5f;
    [SerializeField]
    private float pSmallY = 0.5f;
    [SerializeField]
    private float pSmallX = 1.0f;


    [SerializeField] private Vector3 v3Big;
    [SerializeField] private Vector3 v3Small;
    [SerializeField] private Vector3 currentSize;

    //Game Object
    private Transform pTF;
    private SpriteRenderer pSR;
    [SerializeField] Color cBig;
    [SerializeField] Color cSmall;
    [SerializeField] Sprite spSmall;
    [SerializeField] Sprite spBig;
    public bool isGrabbing = false;

    //KeyCode
    public KeyCode keySize = KeyCode.C;

    //Bool
    public bool sizeBig = false;

    //RayCastHit2D
    RaycastHit2D fitsUp;
    RaycastHit2D fitsDown;

    //New Input System
    private PlayerInput playerInput;

    [SerializeField] private PolygonCollider2D bigPC;
    [SerializeField] private PolygonCollider2D smallPC;
    Vector3 sizeDiffrence;
    Animator playerAni;

    private Sprite currentSprite;

    void Start()
    {
        playerAni = GetComponent<Animator>();
        v3Big = spBig.bounds.extents;
        v3Small = spSmall.bounds.extents;
        currentSize = v3Small;
        sizeDiffrence = v3Big - v3Small;

        pTF = gameObject.transform;
        pSR = GetComponent<SpriteRenderer>();
        //sizeBig = false;

        //New Input System
        playerInput = GetComponent<PlayerInput>();

        currentSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCollider();

        fitsUp = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + currentSize.y+0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.up,10);
        fitsDown= Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - currentSize.y-0.05f), new Vector2(currentSize.x, 0.001f), 0f, Vector2.down,10);
        //Debug.Log("Fits  Up = "+fitsUp.distance + "  Fits Down = "+fitsDown.distance);
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y +currentSize.y + 0.05f), new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - currentSize.y), new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z));

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
                }
                else if (isGrabbing)
                {
                    if (fitsDown.distance > sizeDiffrence.y  + 0.05f|| fitsDown.distance == 0 && fitsDown.collider == null)
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
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
        if(playerAni.)
        if(currentSprite != GetComponent<SpriteRenderer>().sprite)
        {
            Destroy(GetComponent<CapsuleCollider2D>());
            gameObject.AddComponent<CapsuleCollider2D>();
            if(currentSprite.bounds.extents.y > GetComponent<SpriteRenderer>().sprite.bounds.extents.y) sizeDiffrence = currentSprite.bounds.extents - GetComponent<SpriteRenderer>().bounds.extents; 
            else if (currentSprite.bounds.extents.y < GetComponent<SpriteRenderer>().sprite.bounds.extents.y) sizeDiffrence = GetComponent<SpriteRenderer>().bounds.extents - currentSprite.bounds.extents;

            PositionAfterSizeChange();

            currentSprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    void SizeChange()
    {
        
        if (sizeBig==true)
        {
            playerAni.Play("SizeUpGround");
            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pBigX,pBigY,1);
            //bigPC.enabled = true;
            //smallPC.enabled = false;
            //pSR.color = cBig;
            currentSize = v3Big;
        }
        else if (sizeBig == false)
        {
            playerAni.Play("SizeDownGround");
            //PositionAfterSizeChange();
            //pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
            //smallPC.enabled = true;
            //bigPC.enabled = false;
            //pSR.color = cSmall;
            currentSize = v3Small;
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
                    if (fitsDown.distance > v3Big.y - v3Small.y && fitsUp.distance > sizeDiffrence.y)
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
