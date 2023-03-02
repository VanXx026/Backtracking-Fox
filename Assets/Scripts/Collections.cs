using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collections : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(GameManager.instance.player.isTimeLine)
        {
            anim.enabled = true;
        }
        else if(!GameManager.instance.player.isTimeLine)
        {
            anim.enabled = false;
        }
    }
}
