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

    //KeyCode
    public KeyCode keySize = KeyCode.C;

    //Bool
    static public bool sizeBig = false;

    void Start()
    {
        pTF = gameObject.transform;
        pSR = GetComponent<SpriteRenderer>();
        //sizeBig = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keySize))//Key C
        {
            if(sizeBig == false)
            {
                sizeBig = true;// Spilleren er STOR
                SizeChange();
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
            pTF.localScale = new Vector3(pBigX,pBigY,1);
            pSR.color = cBig;
        }
        else if (sizeBig == false)
        {
            pSR.sprite = spSmall;
            pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
            pSR.color = cSmall;
        }
    }
}
