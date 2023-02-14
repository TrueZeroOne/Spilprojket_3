using UnityEngine;
using Cinemachine;

public class CMFindPlayer : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
