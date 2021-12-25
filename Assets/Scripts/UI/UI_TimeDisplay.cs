using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeDisplay : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += UpdateText;
        text = transform.Find("Text").GetComponent<Text>();
        text.text = "Day: 0, Hour: 0";
    }

    public void UpdateText(object source, GameTime gameTime)
    {
        text.text = "Day: " + gameTime.day + ", Hour: " + gameTime.hour;
    }
}
