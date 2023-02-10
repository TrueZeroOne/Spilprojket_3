using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        pTF = gameObject.transform;
        pSR = GetComponent<SpriteRenderer>();
        //sizeBig = false;
    }

    // Update is called once per frame
    void Update()
    {
        fitsUp = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + transform.lossyScale.y * 2), new Vector2(pBigX, 0.001f), 0f, Vector2.up,10);
        fitsDown= Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y * 2), new Vector2(pBigX, 0.001f), 0f, Vector2.down,10);
        //Debug.Log("Fits  Up = "+fitsUp.distance + "  Fits Down = "+fitsDown.distance);
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y + 0.05f), new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        
        if (Input.GetKeyDown(keySize))//Key C
        {
            if(sizeBig == false)
            {
                if (!isGrabbing)
                {
                    if (fitsUp.distance > pBigY * 2 - pTF.lossyScale.y * 5 || fitsUp.distance == 0 && fitsUp.collider == null )
                    {
                        sizeBig = true;// Spilleren er STOR
                        SizeChange();
                    }
                }
                else if (isGrabbing)
                {
                    if (fitsDown.distance > pBigY * 2 - pTF.lossyScale.y * 5 || fitsDown.distance == 0 && fitsDown.collider == null)
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
    void SizeChange()
    {
        if (sizeBig==true)
        {
            pSR.sprite = spBig;
            PositionAfterSizeChange();
            pTF.localScale = new Vector3(pBigX,pBigY,1);
            pSR.color = cBig;
        }
        else if (sizeBig == false)
        {
            pSR.sprite = spSmall;
            PositionAfterSizeChange();
            pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
            pSR.color = cSmall;
        }
    }
    private void PositionAfterSizeChange()
    {
        if (isGrabbing)
        {
            if (sizeBig)
            {
                pTF.position = new Vector3(pTF.position.x, pTF.position.y - (pBigY - pTF.localScale.y), pTF.position.z);
            }
            else if (!sizeBig)
            {
                pTF.position = new Vector3(pTF.position.x, pTF.position.y + (pTF.localScale.y - pSmallY), pTF.position.z);
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
                    pTF.position = new Vector3(pTF.position.x, pTF.position.y + (pBigY - pTF.localScale.y), pTF.position.z);
                }
            }
            else if (!sizeBig)
            {
                if (!GetComponent<PlayerMovement>().grounded)
                {
                    if (fitsDown.distance > pBigY - pTF.lossyScale.y&& fitsUp.distance > pBigY - pTF.lossyScale.y)
                    {
                        return;
                    }
                    else
                    {
                        pTF.position = new Vector3(pTF.position.x, pTF.position.y - (pTF.localScale.y - pSmallY), pTF.position.z);
                    }
                }
                else
                {
                    pTF.position = new Vector3(pTF.position.x, pTF.position.y - (pTF.localScale.y - pSmallY), pTF.position.z);
                }
            }
        }
    }
}
