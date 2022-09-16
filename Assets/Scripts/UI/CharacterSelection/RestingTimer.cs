using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RestingTimer : MonoBehaviour
{
    private TimeSystem timeSystem;
    private Slider timer;
    private CharacterSheet characterSheet;
    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        timer = transform.Find("RestingTimer").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer(float time, CharacterSheet characater)
    {
        timer.gameObject.SetActive(true);
        timeSystem.TickAdded += IncrementTimer;
        timer.value = 0;
        timer.maxValue = time;
        characterSheet = characater;
    }

    private void IncrementTimer(object source, GameTime gameTime)
    {
        timer.value++;
        if(timer.value == timer.maxValue)
        {
            timer.gameObject.SetActive(false);
            var characterPoolController = GameObject.Find("Main UI/QuestDisplayManager/QuestDisplay/CharacterPool").GetComponent<CharacterPoolController>();
            characterPoolController.EndRestingPeriod(characterSheet);
            timeSystem.TickAdded -= IncrementTimer;
        }
    }
}
