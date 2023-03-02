using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate; //决定跟随移动的快慢，1为完全跟随
    private float startPointX;
    private float startPointY;
    public bool lockY; //是否锁定Y轴的视觉差

    private void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    private void Update()
    {
        if (lockY)
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, transform.position.y);
        else //X和Y都跟随镜头相对移动
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY + cam.position.y * moveRate);
    }
}
