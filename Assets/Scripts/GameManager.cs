using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void GameWin()
    {
        player.isStartToRollBack = false;
        AudioManager.instance.PlayWinClip();
        if (LevelManager.instance.levelNow == 4)
        {
            UIManager.instance.OpenCompletePanel();
        }
        else
        {
            UIManager.instance.OpenGameWinPanel();
        }

    }

    public void GameOver()
    {
        player.isStartToRollBack = false;
        AudioManager.instance.PlayLoseClip();
        UIManager.instance.OpenGameLosePanel();
    }
}
