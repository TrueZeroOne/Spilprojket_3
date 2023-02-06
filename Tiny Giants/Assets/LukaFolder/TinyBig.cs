using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyBig : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float pBigHight = 3.5f;
    [SerializeField]
    private float pBigFat = 1.5f;
    [SerializeField]
    private float pSmallHight = 0.5f;
    [SerializeField]
    private float pSmallFat = 1.0f;
    //public Transform pTF = 
    public KeyCode keySize = KeyCode.O;
    public bool sizeBig = false;

    void Start()
    {
        //pTF = GetComponent<Transform>();
        sizeBig = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keySize))
        {
            if(sizeBig == false)
            {
                sizeBig = true;
                
            }
            if(sizeBig == true)
            {
                sizeBig = false;
            }
        }
    }
    /*void SizeChange()
    {
        if (sizeBig==true)
        {
            pTF.sc = pBigHight, pBigFat,1;
        }
    }*/
}
