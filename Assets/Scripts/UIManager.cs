using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject skillPanel;
    public TextMeshProUGUI skillText;
    public GameObject timerPanel;
    public TextMeshProUGUI timerText;
    public GameObject menuButton;
    public GameObject menuPanel;
    public GameObject signPanel;
    public TextMeshProUGUI signText;
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;
    public GameObject completePanel;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void UISceneStart()
    {
        skillPanel.gameObject.SetActive(true);
        timerPanel.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    public void UISceneEnd()
    {
        skillPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
    }

    public void OnClickMenuButton()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenuPanel()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickReturnButton()
    {
        CloseMenuPanel();
    }

    public void OnClickRemakeButton()
    {
        LevelManager.instance.OpenLevel(LevelManager.instance.levelNow);
    }

    public void OnClickExitButton()
    {
        LevelManager.instance.OpenTitle();
    }

    public void OpenSignPanel(string _sign)
    {
        signPanel.SetActive(true);
        ScaleOpen(signPanel.transform);
        signText.text = System.Text.RegularExpressions.Regex.Unescape(_sign);
    }

    public void CloseSignPanel()
    {
        ScaleClose(signPanel.transform);
        // signPanel.SetActive(false);
    }

    public void OpenGameWinPanel()
    {
        gameWinPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseGameWinPanel()
    {
        gameWinPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenGameLosePanel()
    {
        gameLosePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseGameLosePanel()
    {
        gameLosePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickNextLevelButton()
    {
        LevelManager.instance.OpenLevel(LevelManager.instance.levelNow + 1);
    }

    public void OpenCompletePanel()
    {
        completePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseCompletePanel()
    {
        completePanel.SetActive(false);
        Time.timeScale = 1;
    }


    /// DoTween
    public void ScaleOpen(Transform _trans)
    {
        _trans.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine);
    }

    public void ScaleClose(Transform _trans)
    {
        _trans.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutSine);
    }
}
