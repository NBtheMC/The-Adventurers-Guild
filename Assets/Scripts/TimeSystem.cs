using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A global timer with an event hook
public class TimeSystem : MonoBehaviour
{

    private float tickLength = 2.5f; // Length of a tick in seconds. Equal to one in game hour
    private GameTime gameTime; // Current in game time
    public bool timerActive { get; private set; } //Can be read by other classes to determine if timer is running

    public int hoursInDay; // How many hours are there in a day.
    public int ticksperHour; // How many ticks do we want to trigger per hour.
    public int activeHours; // How many hours is the player allowed to play though before the ticker accelerates to the next day?
    public int totalTicksperDay() { return hoursInDay * ticksperHour; } // Get the mathatical ticks per 
    public int totalTicksperActive() { return activeHours * ticksperHour; } // Get the total number of ticks that the player exists in.

    Coroutine timeTrackerCoroutine; // Used to start/stop timer coroutine

    /*
     * Think of this as a list of methods: Other classes can add their methods to this list, and
     * whenever the TickAdded event is fired, all methods attached to this event get called and
     * are passed the current GameTime.
     * Look at UI/TimeDisplay for a simple example of how this works
     */
    public event EventHandler<GameTime> TickAdded;

    public event EventHandler NewDay;

    private void Awake()
    {
        Debug.Log("Starting time");
        StartTimer();
    }

    // While the Coroutine is running, a tick passes every tickLength
    private IEnumerator TimeTracker()
    {
        while (timerActive)
        {
            yield return new WaitForSeconds(tickLength);
            AddTick();
        }
    }

    // Updates gameTime and fire all methods hooked into TickAdded event
    private void AddTick()
    {
        gameTime.hour += 1;
        if (gameTime.hour == 24)
        {
            // NOTE: USING RECAP MANAGER NOW
            // gameTime.day += 1;
            // gameTime.hour = 0;
            //Use GameManager to advance day
            //GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            //gameManager.ChangeScenes("Recap");
            //NewDay(this, EventArgs.Empty);
        }

        if (TickAdded != null)
        {
            TickAdded(this, gameTime);
        }
        Debug.Log("Day: " + gameTime.day + ", Hour: " + gameTime.hour);
    }

    public void StartTimer()
    {
        if(timerActive)
        {
            Debug.LogError("Timer is already active!");
            return;
        }
        timerActive = true;
        timeTrackerCoroutine = StartCoroutine(TimeTracker());
    }

    public void StopTimer()
    {
        timerActive = false;
        StopCoroutine(timeTrackerCoroutine);
    }

    public GameTime getTime() { return gameTime; }
    
    //probably want this to lerp later? just thoughts while working on recap - Naman
    public void SetDay(int currentDay){
        gameTime.day = currentDay;
        gameTime.hour = 0;
    }

    public void StartNewDay()
    {
        NewDay(this, EventArgs.Empty);
    }
}
