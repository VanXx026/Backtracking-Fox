using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.instance.OpenLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
