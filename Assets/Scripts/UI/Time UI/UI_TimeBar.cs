using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeBar : MonoBehaviour
{
    private Slider timeBar;
    private TimeSystem timeSystem;

    private void Awake()
    {
        timeBar = GetComponent<Slider>();
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.TickAdded += FillBar;
        timeBar.value = 0;
    }

    private void FillBar(object src, GameTime gameTime)
    {
        timeBar.value = (float)timeSystem.TotalTicks / timeSystem.TotalTicksperActive;
    }
}
