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
        canvas.transform.Find("MusicSliderContainer").Find("Slider").GetComponent<Slider>().value = GameObject.Find("MusicManager").GetComponent<MusicManager>().GetVolume();
        canvas.transform.Find("SfxVolumeSlider").Find("Slider").GetComponent<Slider>().value = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>().GetVolume();
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
