using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public string level_1_Sign;
    public string level_2_Sign;
    public string level_3_Sign;
    public string level_4_Sign;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string sign = "";
            switch (LevelManager.instance.levelNow)
            {
                case 1:
                    sign = level_1_Sign;
                    break;
                case 2:
                    sign = level_2_Sign;
                    break;
                case 3:
                    sign = level_3_Sign;
                    break;
                case 4:
                    sign = level_4_Sign;
                    break;
            }
            UIManager.instance.OpenSignPanel(sign);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UIManager.instance.CloseSignPanel();
    }
}
