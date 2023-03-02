using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameManager.instance.player.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = GameManager.instance.player.transform;
    }

}
