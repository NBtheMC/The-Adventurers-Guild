using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private TimeSystem timeSystem;
    private CanvasGroup mainCanvasGroup;
    public static bool gamePaused { get; private set; }
    private void Awake()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        mainCanvasGroup = GameObject.Find("MainCanvasGroup").GetComponent<CanvasGroup>();
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
        }
    }
}
