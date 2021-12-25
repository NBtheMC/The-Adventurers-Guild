using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A global timer with an event hook
public class TimeSystem : MonoBehaviour
{

    private float tickLength = 2.5f; // Length of a tick in seconds. Equal to one in game hour
    private GameTime gameTime; // Current in game time
    public bool timerActive = true;
    Coroutine timeTrackerCoroutine; // Used to start/stop timer coroutine

    /*
     * Think of this as a list of methods: Other classes can add their methods to this list, and
     * whenever the TickAdded event is fired, all methods attached to this event get called and
     * are passed the current GameTime.
     * Look at UI/TimeDisplay for a simple example of how this works
     */
    public event EventHandler<GameTime> TickAdded;

    private void Awake()
    {
        timeTrackerCoroutine = StartCoroutine(TimeTracker());
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
            gameTime.day += 1;
            gameTime.hour = 0;
        }

        if (TickAdded != null)
        {
            TickAdded(this, gameTime);
        }
        Debug.Log("Day: " + gameTime.day + ", Hour: " + gameTime.hour);
    }

    public void StartTimer()
    {
        timeTrackerCoroutine = StartCoroutine(TimeTracker());
    }

    public void StopTimer()
    {
        StopCoroutine(timeTrackerCoroutine);
    }
}
