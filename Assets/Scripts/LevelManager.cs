using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int levelNum;
    public List<float> levelTimeList; // 从1开始
    public List<int> levelSkillList; // 从1开始
    public int levelNow;
    private float timer; // 计时器
    public bool isSceneStart; // 如果场景开始了

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        levelNum = levelTimeList.Count - 1;
    }

    private void Update()
    {
        if (GameManager.instance.player != null && GameManager.instance.player.isTimeLine && isSceneStart)
        {
            LevelTimer();
        }
    }

    public void OpenTitle()
    {
        SceneManager.LoadSceneAsync("Title");

        AudioManager.instance.PlayBGMClip();
        UIManager.instance.UISceneEnd();
    }

    public void OpenLevel(int _levelNo)
    {
        SceneManager.LoadSceneAsync(string.Format("Level_0{0}", _levelNo));

        AudioManager.instance.PlayBGMClip();
        levelNow = _levelNo;
        isSceneStart = true;
        UIManager.instance.UISceneStart();
        SetLevelTime();
    }

    // 关卡开始时调用
    public void SetLevelTime()
    {
        timer = levelTimeList[levelNow];
        UIManager.instance.timerText.color = Color.white;
    }

    public void LevelTimer()
    {
        timer -= Time.deltaTime;
        if(timer / levelTimeList[levelNow] <= 0.25f)
        {
            UIManager.instance.timerText.color = Color.red;
        }
        if (timer <= 0)
        {
            timer = 0;
            GameManager.instance.player.isStartToRollBack = true;
            GameManager.instance.player.isTimeLine = false;
        }
        UIManager.instance.timerText.text = string.Format("{0:N2}", timer);
    }
}
