using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip BGMClip;
    public AudioClip getHitClip;
    public AudioClip jumpClip;
    public AudioClip skillClip;
    public AudioClip rollBackClip;
    public AudioClip gainClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    public AudioSource BGMSource;
    public AudioSource playerSource;
    public AudioSource rollBackSource;
    public AudioSource panelSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        BGMSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        rollBackSource = gameObject.AddComponent<AudioSource>();
        panelSource = gameObject.AddComponent<AudioSource>();

        BGMSource.volume = 0.4f;
        playerSource.volume = 0.3f;
        rollBackSource.volume = 0.3f;
        panelSource.volume = 0.3f;
    }

    private void Start()
    {
        PlayBGMClip();
    }

    private void Update()
    {
        if(GameManager.instance.player != null)
        {
            if(GameManager.instance.player.isRollBacking)
            {
                BGMSource.pitch = 2.0f;
            }
            else if(!GameManager.instance.player.isRollBacking)
            {
                BGMSource.pitch = 1.0f;
            }
        }
    }

    public void PlayBGMClip()
    {
        BGMSource.clip = BGMClip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlayGetHitClip()
    {
        playerSource.clip = getHitClip;
        playerSource.Play();
    }

    public void PlayJumpClip()
    {
        playerSource.clip = jumpClip;
        playerSource.Play();
    }

    public void PlayGainClip()
    {
        playerSource.clip = gainClip;
        playerSource.Play();
    }

    public void PlaySkillClip()
    {
        rollBackSource.clip = skillClip;
        rollBackSource.Play();
    }

    public void PlayRollBackClip()
    {
        rollBackSource.clip = rollBackClip;
        rollBackSource.Play();
    }

    public void PlayWinClip()
    {
        panelSource.clip = winClip;
        panelSource.Play();
    }

    public void PlayLoseClip()
    {
        panelSource.clip = loseClip;
        panelSource.Play();
    }
}
