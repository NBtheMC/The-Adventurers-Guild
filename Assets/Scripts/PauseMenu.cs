using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private TimeSystem timeSystem;
    private CanvasGroup mainCanvasGroup;
    private CanvasGroup pauseCanvasGroup;
    public static bool gamePaused { get; private set; }
    private void Awake()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        mainCanvasGroup = GameObject.Find("QuestDisplayManager").GetComponent<CanvasGroup>();
        pauseCanvasGroup = GetComponent<CanvasGroup>();
        gamePaused = false;

    }

    private void Start()
    {
        var canvas = pauseCanvasGroup.transform.Find("Canvas");
        var musicSlider = canvas.transform.Find("MusicSliderContainer").Find("Slider").GetComponent<Slider>();
        var sfxSlider = canvas.transform.Find("SfxVolumeSlider").Find("Slider").GetComponent<Slider>();
        var musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        var sfxManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();

        musicSlider.value = musicManager.GetVolume();
        sfxSlider.value = sfxManager.GetVolume();

        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(musicManager.ChangeVolume);

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener(sfxManager.ChangeVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void Pause() {
        gamePaused = true;
        timeSystem.StopTimer();
    }

    public void Play() {
        gamePaused = false;
        timeSystem.StartTimer();
    }



    private void TogglePause()
    {
        gamePaused = !gamePaused;
        ToggleTimer(!gamePaused);
        TogglePauseDisplay(gamePaused);
        mainCanvasGroup.interactable = !gamePaused;
    }

    private void ToggleTimer(bool active)
    {
        if (active)
            timeSystem.StartTimer();
        else
            timeSystem.StopTimer();
    }

    private void TogglePauseDisplay(bool active)
    {
        pauseCanvasGroup.transform.Find("Canvas").gameObject.SetActive(active);
    }
}
