using System;
using System.Collections;
using UnityEngine;

// A global timer with an event hook
public class TimeSystem : MonoBehaviour
{
    #region PrivateMembers
    private float _tickLength = 2f; // Length of a tick in seconds. Equal to one in game hour
    private GameTime _gameTime; // Current in game time
    [SerializeField]
    private int _hoursInDay; // How many hours are there in a day.
    [SerializeField]
    private int _ticksperHour; // How many ticks do we want to trigger per hour.
    [SerializeField]
    private int _activeHours; // How many hours is the player allowed to play though before the ticker accelerates to the next day?
    #endregion

    #region Properties
    public int TicksPerHour { get => _ticksperHour; }
    public int TotalTicksperDay { get => _hoursInDay * _ticksperHour; } // Get the mathatical ticks per active day.
    public int TotalTicksperActive { get => _activeHours * _ticksperHour; } // Get the total number of ticks that the player exists in.
    public int TotalTicks { get => _gameTime.hour * _ticksperHour + _gameTime.tick; } //how many ticks have occurred for this day
    public GameTime GameTime { get => _gameTime; }
    public bool TimeActive { get; private set; } //Can be read by other classes to determine if timer is running
    #endregion

    Coroutine timeTrackerCoroutine; // Used to start/stop timer coroutine

    /*
     * Think of this as a list of methods: Other classes can add their methods to this list, and
     * whenever the TickAdded event is fired, all methods attached to this event get called and
     * are passed the current GameTime.
     * Look at UI/TimeDisplay for a simple example of how this works
     */
    public event EventHandler<GameTime> TickAdded; // Invoked every tick.
    public event EventHandler<GameTime> EndOfDay; // Invoked when active hours are over.
    public event EventHandler<GameTime> NewHour; // Invoked when there's a new hour.

    public event EventHandler<GameTime> NewDay;

    private void Awake()
    {
    }

    // While the Coroutine is running, a tick passes every tickLength
    private IEnumerator TimeTracker()
    {
        while (TimeActive)
        {
            yield return new WaitForSeconds(_tickLength);
            AddTick();
        }
    }

    // Updates gameTime and fire all methods hooked into TickAdded event
    private void AddTick()
    {
        _gameTime.tick += 1;
        if (_gameTime.tick >= _ticksperHour)
        {
            _gameTime.tick = 0;
            _gameTime.hour += 1;
            NewHour?.Invoke(this, _gameTime);
            if (_gameTime.hour >= _activeHours)
            {
                EndOfDay?.Invoke(this, _gameTime);
                StopTimer();
                GameObject.Find("RecapDisplay").GetComponent<RecapManager>().StartRecap();
            }
        }

        TickAdded?.Invoke(this, _gameTime);
        //Debug.Log("Day: " + gameTime.day + ", Hour: " + gameTime.hour);
    }

    /// <summary>
    /// Advances the time by the given number of ticks. 
    /// </summary>
    /// <param name="ticksToAdd">Number of ticks to advance the time by</param>
    public void AddTicks(int ticksToAdd)
    {
        if (!TimeActive)
            Debug.LogError("Trying to advance time when timer is paused");

        else
        {
            //May or may not be necessary. Temporarily stop timer since we're manually adding ticks to the current time
            StopTimer();
            int totalTicks = GameTime.hour * TicksPerHour + GameTime.tick;
            if (totalTicks + ticksToAdd > TotalTicksperActive)
            {
                for (int i = 0; i < TotalTicksperActive - totalTicks; i++)
                    AddTick();
            }
            else
            {
                for (int i = 0; i < ticksToAdd; i++)
                    AddTick();
            }

            StartTimer();
        }
    }

    /// <summary>
    /// Advances the time by the given number of hours.
    /// </summary>
    /// <param name="hours">Number of hours to advance the time by. TicksPerHour % hours MUST equal 0</param>
    public void AddTicks(double hours)
    {
        Debug.Assert(TicksPerHour % hours == 0, $"TicksPerHour % hours must equal 0: {TicksPerHour} % {hours} = {TicksPerHour % hours}");

        int ticksToAdd = (int)(TicksPerHour * hours);
        AddTicks(ticksToAdd);
    }

    public void StartTimer()
    {
        if(TimeActive)
        {
            Debug.LogError("Timer is already active!");
            return;
        }
        TimeActive = true;
        timeTrackerCoroutine = StartCoroutine(TimeTracker());
    }

    public void StopTimer()
    {
        TimeActive = false;
        StopCoroutine(timeTrackerCoroutine);
    }

    /// <summary>
    /// Defunct function. Do not use unless neccessary.
    /// </summary>
    public void SetDay(int number) {
        _gameTime.day = number;
    }

    public void StartNewDay()
    {
        NewDay(this, _gameTime);
        while(_gameTime.hour < _hoursInDay) {
            _gameTime.tick++;
            if (_gameTime.tick >= _ticksperHour)
            {
                _gameTime.tick = 0;
                _gameTime.hour += 1;
                NewHour?.Invoke(this, _gameTime);
            }
        }
        _gameTime.hour = 0;
        _gameTime.day += 1;
        StartTimer();
    }

    
}
