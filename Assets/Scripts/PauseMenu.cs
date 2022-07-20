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
        mainCanvasGroup = GameObject.Find("QuestDisplayManager").GetComponent<CanvasGroup>();
        gamePaused = false;
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
        mainCanvasGroup.transform.Find("PauseCanvas").gameObject.SetActive(active);
    }
}
