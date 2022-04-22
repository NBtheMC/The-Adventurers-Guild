using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private TimeSystem timeSystem;
    private CanvasGroup mainCanvasGroup;
    private Canvas pauseCanvas;
    private Slider volume1;

    public event EventHandler<float> volume1Change;
    public static bool gamePaused { get; private set; }
    private void Awake()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        mainCanvasGroup = GameObject.Find("QuestDisplayManager").GetComponent<CanvasGroup>();
        pauseCanvas = transform.Find("PauseCanvas").GetComponent<Canvas>();
        volume1 = pauseCanvas.transform.Find("Setting1").Find("Slider").GetComponent<Slider>();
    }

    private void Start()
    {
        volume1.onValueChanged.AddListener(delegate { volume1Change(this, volume1.value); });
        volume1.value = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>().GetAudioSrcVolume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (timeSystem.timerActive)
                timeSystem.StopTimer();
            else
                timeSystem.StartTimer();
            gamePaused = !timeSystem.timerActive;
            mainCanvasGroup.interactable = !gamePaused;
            pauseCanvas.gameObject.SetActive(gamePaused);
        }
    }
}
