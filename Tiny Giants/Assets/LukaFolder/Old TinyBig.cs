using UnityEngine;

public class Old_TinyBig : MonoBehaviour
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
    [SerializeField] private Color cBig;
    [SerializeField] private Color cSmall;

    //KeyCode
    public KeyCode keySize = KeyCode.C;

    //Bool
    public bool sizeBig = false;

    private void Start()
    {
        pTF = gameObject.transform;
        pSR = GetComponent<SpriteRenderer>();
        //sizeBig = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(keySize))//Key C
        {
            if (sizeBig == false)
            {
                sizeBig = true;// Spilleren er STOR
                SizeChange();
                /*if (sizeBig == true)
                {
                    pTF.localScale = new Vector3(pBigX, pBigY, 1);
                }*/
                pSR.color = cBig;

            }
            else if (sizeBig == true)
            {
                sizeBig = false;// Spilleren er LILLE
                SizeChange();
                /*if (sizeBig == false)
                {
                    pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
                }*/
                pSR.color = cSmall;
            }
        }
    }
    private void SizeChange()
    {
        if (sizeBig == true)
        {
            pTF.localScale = new Vector3(pBigX, pBigY, 1);

        }
        else if (sizeBig == false)
        {
            pTF.localScale = new Vector3(pSmallX, pSmallY, 1);
        }
    }
}