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
    private float pSmallHight = 0.5f;
    [SerializeField]
    private float pSmallFat = 1.0f;

    //Game Object
    private Transform pTF;

    //KeyCode
    public KeyCode keySize = KeyCode.C;
    public bool sizeBig = false;

    void Start()
    {
        pTF = gameObject.transform;
        sizeBig = false;
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

            }
            if(sizeBig == true)
            {
                sizeBig = false;// Spilleren er LILLE
                SizeChange();
            }
        }
    }
    void SizeChange()
    {
        if (sizeBig==true)
        {
            pTF.localScale = new Vector3(pBigX,pBigY,1);
            
        }
        if (sizeBig == false)
        {
            pTF.localScale = new Vector3(pBigX, pBigY, 1);
        }
    }
}
